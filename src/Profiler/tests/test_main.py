import pytest
from fastapi.testclient import TestClient
from app.main import app

@pytest.fixture
def mock_request_data():
    return {
        "purchase_history": [[1, 2, 3], [4, 5, 6]],
        "product_frequencies": [{"1": 5, "2": 3, "3": 1}, {"4": 2, "5": 7, "6": 1}],
        "labels": [1, 5],
        "top_k": 3,
        "latest": True
    }

client = TestClient(app)

def test_read_root():
    response = client.get("/")
    assert response.status_code == 200
    assert response.json() == {"message": "Welcome to my Profiler API project!"}

def test_read_item():
    response = client.get("/items/42?q=test")
    assert response.status_code == 200
    assert response.json() == {"item_id": 42, "query": "test"}


def test_predict(mock_request_data):
    response = client.post("/predict/", json=mock_request_data)

    assert response.status_code == 200
    response_data = response.json()
    assert len(response_data.get("predictions")) == 1

def test_invalid_request():
    # Send an invalid request (missing required fields)
    response = client.post("/predict/", json={})
    
    assert response.status_code == 422  # Unprocessable Entity
    assert "detail" in response.json()
