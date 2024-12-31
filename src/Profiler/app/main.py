import logging
from fastapi import FastAPI
from app.routes import router
from app.schemas import PredictionRequest, PredictionResponse
from app.services import predict_purchase

# Configure logging to INFO level
logging.basicConfig(level=logging.INFO)

# Initialize the FastAPI application
app = FastAPI(title="Profiler API", version="1.0.0")

# Include API routes from the router
app.include_router(router)

@app.get("/")
def read_root():
    return {"message": "Welcome to my Profiler API project!"}

@app.post("/predict/", response_model=PredictionResponse)
async def predict(request: PredictionRequest):
    # Call the service to handle prediction logic
    return await predict_purchase(request)
