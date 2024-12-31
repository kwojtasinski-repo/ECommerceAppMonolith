import numpy as np
from tensorflow.keras.preprocessing.sequence import pad_sequences
from tensorflow.keras.models import Model
from tensorflow.keras.layers import Embedding, LSTM, Dense, Concatenate, Input, Bidirectional, Dropout
from typing import List, Dict

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

def preprocess_data(purchase_history: List[List[int]], product_frequencies: List[Dict[int, int]], product_to_index: Dict[int, int], max_seq_len: int) -> (np.ndarray, np.ndarray):
    # Adjust purchase history by replacing product IDs with indices
    adjusted_purchase_history = [
        [product_to_index.get(product, product_to_index['PAD']) for product in week] for week in purchase_history
    ]

    # Adjust frequency data
    frequency_data = [
        [product_frequencies[week_idx].get(product, 0) for product in week] for week_idx, week in enumerate(purchase_history)
    ]
    
    # Pad sequences
    padded_X = pad_sequences(adjusted_purchase_history, maxlen=max_seq_len, padding='pre', dtype='int32', value=-1)
    padded_frequencies = pad_sequences(frequency_data, maxlen=max_seq_len, padding='pre', dtype='float32', value=0)

    return padded_X, padded_frequencies

def generate_predictions(model: Model, padded_X: np.ndarray, padded_frequencies: np.ndarray, top_k: int) -> List[List[str]]:
    # Mock predictions (replace with model.predict in a real setup)
    mock_predictions = np.random.rand(len(padded_X), model.output_shape[1])
    top_predictions = np.argsort(mock_predictions, axis=1)[:, -top_k:][:, ::-1]
    
    return top_predictions
