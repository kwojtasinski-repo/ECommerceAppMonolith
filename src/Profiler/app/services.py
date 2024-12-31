import numpy as np
from tensorflow.keras.preprocessing.sequence import pad_sequences
from tensorflow.keras.models import Model
from tensorflow.keras.layers import Embedding, LSTM, Dense, Concatenate, Input, Bidirectional, Dropout
from typing import List, Dict
from app.schemas import PredictionRequest


# Main function that orchestrates the prediction
async def predict_purchase(request: PredictionRequest) -> Dict:
    purchase_history = request.purchase_history
    product_frequencies = request.product_frequencies
    top_k = request.top_k
    latest = request.latest
    
    # Step 1: Prepare the product mappings and pre-process data
    product_to_index, index_to_product = generate_product_mappings(purchase_history)
    max_seq_len = calculate_max_sequence_length(purchase_history)
    
    padded_X, padded_frequencies = preprocess_data(purchase_history, product_frequencies, product_to_index, max_seq_len)
    
    # Step 2: Create the model
    input_dim = len(product_to_index)
    output_dim = len(product_to_index)  # Number of unique products
    model = create_optimized_model(input_dim, output_dim, max_seq_len)
    
    # Step 3: Generate predictions
    top_predictions = generate_predictions(model, padded_X, padded_frequencies, top_k)
    
    # Step 4: Format the response
    results = format_predictions(top_predictions, index_to_product, purchase_history, latest)
    
    return {"predictions": results}


# Helper function to create product-to-index and index-to-product mappings
def generate_product_mappings(purchase_history: List[List[int]]) -> (Dict[int, int], Dict[int, int]):
    unique_products = set([product for week in purchase_history for product in week])
    product_to_index = {product: idx for idx, product in enumerate(unique_products)}
    product_to_index['PAD'] = -1  # Padding token
    index_to_product = {idx: product for product, idx in product_to_index.items()}
    return product_to_index, index_to_product


# Helper function to calculate the maximum sequence length
def calculate_max_sequence_length(purchase_history: List[List[int]]) -> int:
    return max(len(week) for week in purchase_history) + 1


# Preprocessing data - Adjust purchase history and frequency data, then pad sequences
def preprocess_data(purchase_history: List[List[int]], product_frequencies: List[Dict[int, int]],
                    product_to_index: Dict[int, int], max_seq_len: int) -> (np.ndarray, np.ndarray):
    adjusted_purchase_history = adjust_purchase_history(purchase_history, product_to_index)
    frequency_data = adjust_frequency_data(purchase_history, product_frequencies)
    
    padded_X = pad_sequences(adjusted_purchase_history, maxlen=max_seq_len, padding='pre', dtype='int32', value=-1)
    padded_frequencies = pad_sequences(frequency_data, maxlen=max_seq_len, padding='pre', dtype='float32', value=0)
    
    return padded_X, padded_frequencies


# Helper function to adjust purchase history by replacing product IDs with indices
def adjust_purchase_history(purchase_history: List[List[int]], product_to_index: Dict[int, int]) -> List[List[int]]:
    return [
        [product_to_index.get(product, product_to_index['PAD']) for product in week] for week in purchase_history
    ]


# Helper function to adjust frequency data based on purchase history
def adjust_frequency_data(purchase_history: List[List[int]], product_frequencies: List[Dict[int, int]]) -> List[List[int]]:
    return [
        [product_frequencies[week_idx].get(product, 0) for product in week] for week_idx, week in enumerate(purchase_history)
    ]


# Function to create the optimized model
def create_optimized_model(input_dim: int, output_dim: int, max_seq_len: int) -> Model:
    product_input = Input(shape=(max_seq_len,), dtype='int32', name='product_input')
    product_embedding = Embedding(input_dim=input_dim, output_dim=32, input_length=max_seq_len)(product_input)
    lstm_out = Bidirectional(LSTM(64, return_sequences=False))(product_embedding)
    
    frequency_input = Input(shape=(max_seq_len,), dtype='float32', name='frequency_input')
    combined = Concatenate()([lstm_out, frequency_input])
    combined = Dropout(0.2)(combined)
    dense_out = Dense(128, activation='relu')(combined)
    output = Dense(output_dim, activation='softmax')(dense_out)
    
    model = Model(inputs=[product_input, frequency_input], outputs=output)
    model.compile(optimizer='adam', loss='sparse_categorical_crossentropy', metrics=['accuracy'])
    return model


# Generate predictions (mock predictions for now)
def generate_predictions(model: Model, padded_X: np.ndarray, padded_frequencies: np.ndarray, top_k: int) -> List[List[int]]:
    # Mock predictions (replace with model.predict in a real setup)
    mock_predictions = np.random.rand(len(padded_X), model.output_shape[1])
    top_predictions = np.argsort(mock_predictions, axis=1)[:, -top_k:][:, ::-1]
    return top_predictions


# Format the predictions into the response format
def format_predictions(top_predictions: np.ndarray, index_to_product: Dict[int, int], purchase_history: List[List[int]], latest: bool) -> List[Dict]:
    results = []
    for i, pred in enumerate(top_predictions):
        product_predictions = [index_to_product.get(idx) for idx in pred]
        if latest:
            results = [{"week": len(purchase_history), "predictions": product_predictions}]
        else:
            results.append({"week": i + 1, "predictions": product_predictions})
    return results
