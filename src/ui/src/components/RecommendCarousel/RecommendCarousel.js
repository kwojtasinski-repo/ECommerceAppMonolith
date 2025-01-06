import React, { useState } from "react";
import { Carousel, Button } from "react-bootstrap";
import styles from "./RecommendCarousel.module.css";

const RecommendedCarousel = () => {
  const [isVisible, setIsVisible] = useState(true);
  const recommendedProducts = [
    {
      id: 1,
      name: "Produkt 1",
      image: "https://via.placeholder.com/100x100",
      link: "/product/1",
    },
    {
      id: 2,
      name: "Produkt 2",
      image: "https://via.placeholder.com/100x100",
      link: "/product/2",
    },
    {
      id: 3,
      name: "Produkt 3",
      image: "https://via.placeholder.com/100x100",
      link: "/product/3",
    },
  ];

  return (
    <>
        {recommendedProducts && recommendedProducts.length > 0 ?
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
                        {recommendedProducts.map((product) => (
                        <Carousel.Item key={product.id}>
                            <div className={`d-flex flex-column align-items-center ${styles.carouselItem}`}>
                            <img
                                src={product.image}
                                alt={product.name}
                                style={{ maxWidth: "100px", maxHeight: "100px", borderRadius: "10px" }}
                            />
                            <h6 className="mt-2">{product.name}</h6>
                            <Button
                                href={product.link}
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
