from tensorflow.keras.models import Model
from tensorflow.keras.layers import Input, Embedding, LSTM, Dense, Concatenate, Bidirectional, Dropout

def create_model(input_dim: int, output_dim: int, max_seq_len: int) -> Model:
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
