import { useEffect, useState } from "react";
import axios from "../../axios-setup";
import { mapToCurrencies } from "../../helpers/mapper";

function Currencies(props) {
    const [currencies, setCurrencies] = useState([]);

    const getCurrencies = async () => {
        const response = await axios.get("/currencies-module/currencies");
        setCurrencies(mapToCurrencies(response.data));
    }

    useEffect(() => {
        getCurrencies();
    }, []);

    return (
        <div>
            <table className="table table-striped">
                <thead>
                    <tr>
                        <th scope="col">Kod waluty</th>
                        <th scope="col">Opis</th>
                    </tr>
                </thead>
                <tbody>
                    {currencies.map(c => (
                        <tr key={new Date().getTime() + Math.random()}>
                            <td>{c.code}</td>
                            <td>{c.description}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    )
}

export default Currencies;