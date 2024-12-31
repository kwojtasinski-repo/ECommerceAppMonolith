from pydantic import BaseModel
from typing import List, Dict, Optional

class Item(BaseModel):
    name: str
    description: Optional[str] = None
    price: float
    tax: Optional[float] = None

class PredictionRequest(BaseModel):
    purchase_history: List[List[int]]
    product_frequencies: List[Dict[int, int]]
    top_k: Optional[int] = 3  # Default is 3 predictions
    latest: Optional[bool] = True  # Default is to show the latest predictions
