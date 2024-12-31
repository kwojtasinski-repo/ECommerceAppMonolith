from fastapi import FastAPI
from app.routes import router

# Initialize the FastAPI application
app = FastAPI(title="Profiler API", version="1.0.0")

# Include API routes from the router
app.include_router(router)

@app.get("/")
def read_root():
    return {"message": "Welcome to my Profiler API project!"}
