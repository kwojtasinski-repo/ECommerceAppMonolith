from pydantic import BaseModel, field_validator, model_validator
from typing import List, Dict, Optional

class Item(BaseModel):
    name: str
    description: Optional[str] = None
    price: float
    tax: Optional[float] = None

class PredictionRequest(BaseModel):
    purchase_history: List[List[int]]
    product_frequencies: List[Dict[int, int]]
    top_k: Optional[int] = -1  # Default is -1 predictions which means all
    latest: Optional[bool] = True  # Default is to show the latest predictions
    labels: List[int]

    @field_validator("top_k", mode="before")
    def validate_top_k(cls, value, _):
        if value != -1:  # Validate only if top_k is not -1
            if value < 1:
                raise ValueError("top_k must be at least 1.")
        return value

    @model_validator(mode="after")
    def validate_relationships(cls, values):
        labels = values.labels
        purchase_history = values.purchase_history
        top_k = values.top_k
        product_frequencies = values.product_frequencies

        if labels and top_k != -1 and top_k > len(labels):
            raise ValueError(f"top_k cannot exceed the number of labels ({len(labels)}).")
        if labels and purchase_history and len(labels) != len(purchase_history):
            raise ValueError("The number of labels should have the same length like purchase_history.")
        if len(product_frequencies) != len(purchase_history):
            raise ValueError("The number of product_frequencies should have the same length like purchase_history.")
        
        for history_idx, (history, freq) in enumerate(zip(purchase_history, product_frequencies)):
            for position, item in enumerate(history):
                if item not in freq:
                    raise ValueError(
                        f"product_frequencies array for purchase_history index {history_idx}, position {position} "
                        f"has missing item {item}."
                    )
                if freq.get(item) <= 0:
                    raise ValueError(
                        f"Item {item} in purchase_history index {history_idx}, position {position} must have a positive frequency."
                    )

        return values

class PredictionResult(BaseModel):
    week: int
    predictions: Optional[List[int]]

class PredictionResponse(BaseModel):
    predictions: List[PredictionResult]