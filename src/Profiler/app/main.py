from fastapi import FastAPI
from app.routes import router

# Initialize the FastAPI application
app = FastAPI(title="Profiler API", version="1.0.0")

# Include API routes from the router
app.include_router(router)

@app.get("/")
def read_root():
    return {"message": "Welcome to my Profiler API project!"}

@app.post("/predict/")
async def predict(request: PredictionRequest):
    # Extract input data from request
    purchase_history = request.purchase_history
    product_frequencies = request.product_frequencies
    top_k = request.top_k
    latest = request.latest

    # Step 1: Create a mapping of product IDs to indices
    unique_products = set([product for week in purchase_history for product in week])
    product_to_index = {product: idx for idx, product in enumerate(unique_products)}
    product_to_index['PAD'] = -1  # Padding token
    
    # Reverse mapping for predictions
    index_to_product = {idx: product for product, idx in product_to_index.items()}

    # Step 2: Preprocess data (adjust purchase history and frequency data)
    max_seq_len = max(len(week) for week in purchase_history) + 1
    padded_X, padded_frequencies = preprocess_data(purchase_history, product_frequencies, product_to_index, max_seq_len)

    # Step 3: Create and train the model
    input_dim = len(product_to_index)
    output_dim = len(unique_products)
    model = create_optimized_model(input_dim=input_dim, output_dim=output_dim, max_seq_len=max_seq_len)

    # Generate predictions
    top_predictions = generate_predictions(model, padded_X, padded_frequencies, top_k)

    # Format the response
    results = []
    for i, pred in enumerate(top_predictions):
        product_predictions = [index_to_product[idx] for idx in pred]
        if latest:
            results = [{"week": len(purchase_history), "predictions": product_predictions}]
        else:
            results.append({"week": i + 1, "predictions": product_predictions})

    return {"predictions": results}