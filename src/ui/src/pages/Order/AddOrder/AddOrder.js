import useAuth from "../../../hooks/useAuth";
import ContactForm from "../../Profile/ContactData/ContactForm";
import axios from "../../../axios-setup";
import AddContact from "../../Profile/ContactData/AddContact/AddContact";

function AddOrder(props) {
    const [auth] = useAuth();

    const submit = async (form) => {
    }
    // dodac dane osobowe juz istniejace
    // jesli gosc nie chce to moze dodac nowe dane osobowe
    // walidowac wybranie danych i komunikat najlepiej wyswietlic ze nie ma zaznaczonych danych (tylko nie notyfikacje)
    // po dodaniu odswiezyc komponent
    // po pomyslnym dodaniu przekierowanie na "podsumowanie" ktore pozwala na edycje lub platnosc
    return (
        <div>
            <AddContact />
        </div>
    )
}

export default AddOrder;