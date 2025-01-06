import React, { useContext, useState } from "react";
import { Carousel, Button } from "react-bootstrap";
import styles from "./RecommendCarousel.module.css";
import ReducerContext from "../../context/ReducerContext";

const RecommendedCarousel = () => {
  const [isVisible, setIsVisible] = useState(true);
  const context = useContext(ReducerContext);

  return (
    <>
        {context.state.recommendationProducts && context.state.recommendationProducts.length > 0 ?
            <div className="mt-3">
                <div className="d-flex justify-content-center align-items-center mb-2">
                    <h5 className="mb-0 me-2">Polecane produkty</h5>
                    <Button
                        variant="secondary"
                        size="sm"
                        onClick={() => setIsVisible(!isVisible)}
                    >
                        {isVisible ? "Ukryj" : "Pokaż"}
                    </Button>
                </div>
                {isVisible ?
                    <Carousel data-bs-theme="dark">
                        {context.state.recommendationProducts.map((product) => (
                        <Carousel.Item key={product.productId}>
                            <div className={`d-flex flex-column align-items-center ${styles.carouselItem}`}>
                            <img
                                src={product.imageUrl}
                                alt={product.name}
                                style={{ maxWidth: "100px", maxHeight: "100px", borderRadius: "10px" }}
                            />
                            <h6 className="mt-2">{product.name}</h6>
                            <Button
                                href={`/items/${product.productSaleId}`}
                                variant="primary"
                                size="sm"
                                className="mt-2"
                            >
                                Zobacz więcej
                            </Button>
                            </div>
                        </Carousel.Item>
                        ))}
                    </Carousel>
                    : null
                }
            </div>
            : null
        }
    </>
  );
};

export default RecommendedCarousel;
