export function mapCodeToMessage(code) {
    switch (code) {
        case 'invalid_password' : 
            return "Niepoprawne hasło. Hasło powinno zawierać przynajmniej 8 znaków, w tym jedną dużą literę i jedną liczbę";
        case 'user_not_active' :
            return "Użytkownik nie jest aktywny";
        case 'email_in_use' :
            return "Podany email jest już używany";
        case 'invalid_credentials' :
            return "Niepoprawne dane logowania, sprawdź czy email i hasło są poprawne";
        case 'passwords_are_not_same' :
            return 'Hasła nie są identyczne';
        case 'user_not_found' :
            return 'Nie znaleziono użytkownika';
        case 'address_cannot_be_null' :
            return "Adres nie może być pusty";
        case 'address_not_found' :
            return "Nie znaleziono adresu";
        case 'building_number_cannot_be_empty' :
            return "Numer budynku nie może być pusty";
        case 'building_number_too_long' :
            return 'Niepoprawna liczba \'numer budynku\'';
        case 'city_name_cannot_be_empty' :
            return 'Nazwa miasta nie może być pusta';
        case 'city_name_too_long' :
            return 'Za długa nazwa miasta';
        case 'city_name_too_small' :
            return 'Za krótka nazwa miasta';
        case 'country_name_cannot_be_empty' :
            return 'Nazwa Państwa nie może być pusta';
        case 'country_name_too_long' :
            return 'Nazwa Państwa jest za krótka';
        case 'country_name_too_small' :
            return 'Nazwa Państaw jest za długa';
        case 'invalid_zip_code_format' :
            return 'Niepoprawny kod pocztowy';
        case 'locale_number_cannot_be_empty' :
            return 'Numer lokalu nie może być pusty';
        case 'locale_number_too_long' :
            return 'Numer lokalu jest za długi';
        case 'street_name_cannot_be_empty' :
            return 'Nazwa ulicy nie może być pusta';
        case 'street_name_too_long' :
            return 'Nazwa ulicy jest za długa';
        case 'street_name_too_small' :
            return 'Nazwa ulicy jest za krótka';
        case 'zip_code_cannot_be_empty' :
            return 'Kod pocztowy nie może być pusty';
        case 'zip_code_too_long' :
            return 'Kod pocztowy jest za długi';
        case 'company_name_cannot_be_null' :
            return 'Nazwa firmy nie może być pusta';
        case 'company_name_too_long' :
            return 'Nazwa firmy jest za długa';
        case 'company_name_too_short' :
            return 'Nazwa firmy jest za krótka';
        case 'customer_cannot_be_null' :
            return 'Dane kontaktowe nie mogą być puste';
        case 'customer_not_found' :
            return 'Nie znaleziono danych kontaktowych';
        case 'first_name_cannot_be_null' :
            return 'Imię nie może być puste';
        case 'first_name_too_long' :
            return 'Imię jest za długie';
        case 'first_name_too_short' :
            return 'Imię jest za krótkie';
        case 'invalid_phone_number_format' :
            return 'Niepoprawny numer telefonu';
        case 'last_name_cannot_be_null' :
            return 'Nazwisko nie może być puste';
        case 'last_name_too_long' :
            return 'Nazwisko jest za długie';
        case 'last_name_too_short' :
            return 'Nazwisko jest za krótkie';
        case 'nip_cannot_be_null' :
            return 'NIP nie może być pusty';
        case 'nip_too_long' :
            return 'NIP jest za długi';
        case 'nip_too_short' :
            return 'NIP jest za krótki';
        case 'phone_number_cannot_be_null' :
            return 'Numer telefonu nie może być pusty';
        case 'phone_number_too_long' :
            return 'Numer telefonu jest za długi';
        case 'phone_number_too_short': 
            return 'Numer telefonu jest za krótki';
        case 'cannot_delete_currency' :
            return 'Nie można usunąć waluty, gdyż zawiera już kursy walut';
        case 'cannot_find_currency_rate' :
            return 'Nie znaleziono kursu';
        case 'currency_not_found' :
            return 'Nie znaleziono waluty';
        case 'currency_rate_not_found' :
            return 'Nie znaleziono kursu';
        case 'rate_not_found' :
            return 'Nie znaleziono kursu';
        case 'brand_cannot_be_deleted' :
            return 'Firma nie może zostać usunięta, gdyż przedmioty ją zawierają';
        case 'brand_name_too_long' :
            return 'Nazwa firmy jest za długa';
        case 'brand_name_too_short' :
            return 'Nazwa firmy jest za krótka';
        case 'brand_not_found' :
            return 'Nie znaleziono firmy';
        case 'cannot_create_item_sale_without_images' :
            return 'Nie można wystawić przedmiotu bez obrazków';
        case 'cannot_create_item_sale_without_main_image' :
            return 'Nie można wystawić przedmiotu bez głównego obrazka';
        case 'cannot_delete_item' :
            return 'Nie można usunąć przedmiotu, gdyż już został wystawiony';
        case 'cannot_update_item' :
            return 'Nie można aktualizować przedmiotu, gdyż już został wystawiony';
        case 'images_cannot_be_empty' :
            return 'Obrazki nie mogą być puste';
        case 'invalid_brand_name' :
            return 'Niepoprawna nazwa firmy';
        case 'invalid_files':
            return 'Niepoprawne pliki, sprawdź format, wielkość';
        case 'invalid_type_name' :
            return 'Niepoprawny nazwa typu przedmiotu';
        case 'item_cost_cannot_be_negative' :
            return 'Koszt przedmiotu nie może być ujemny';
        case 'item_images_not_allowed_more_than_one_main_image' :
            return 'Nie można mieć więcej niż 1 główny obrazek';
        case 'item_name_cannot_be_empty' :
            return 'Nazwa przedmiotu nie może być pusta';
        case 'item_name_too_long' :
            return 'Nazwa przedmiotu jest za długa';
        case 'item_name_too_short' :
            return 'Nazwa przedmiotu jest za krótka';
        case 'item_not_found' :
            return 'Nie znaleziono przedmiot';
        case 'item_sale_not_found' :
            return 'Przedmiot wystawiony nie został znaleziony';
        case 'tags_cannot_be_null' :
            return 'Tagi nie mogą być puste';
        case 'tags_too_long' :
                return 'Za duża liczba tagów';
        case 'type_cannot_be_deleted' :
            return 'Typ przedmiotu nie może być usunięty, gdyż jest używany przez przedmioty';
        case 'type_name_too_long' :
            return 'Nazwa typu przdmiotu jest za długa';
        case 'type_name_too_short' :
            return 'Nazwa typu jest za krótka';
        case 'type_not_found' :
            return 'Typ nie został znaleziony';
        case 'urls_cannot_be_null' :
            return 'Obrazki nie mogą być puste';
        case 'brand_cannot_be_null' :
            return 'Firma nie może być pusta';
        case 'brand_name_cannot_be_empty' :
            return 'Nazwa firmy nie może być pusta';
        case 'cost_cannot_be_negative' :
            return 'Koszt przedmiotu nie może być ujemny';
        case 'currency_code_cannot_be_null' :
            return 'Kod waluty nie może być pusty';
        case 'image_name_cannot_be_null' :
            return 'Nazwa obrazku nie może być pusta';
        case 'invalid_currency' : 
            return 'Niepoprawny kod waluty';
        case 'item_cannot_be_null' :
            return 'Przedmiot nie może być pusty';
        case 'type_name_cannot_be_empty' :
            return 'Nazwa typu przedmiotu nie może być pusta';
        case 'cannot_find_all_currencies' :
            return 'Wystąpił błąd podczas przeliczania kosztów zakupu przedmiotu';
        case 'cannot_refresh_cost_when_item_cart_is_null' :
            return 'Wystąpił błąd podczas przeliczania kosztów zakupu przedmiotu';
        case 'customer_cannot_be_empty' :
            return 'Dane kontaktowe nie mogą być puste';
        case 'order_approved_date_cannot_be_less_than_create_order_date' :
            return 'Data zatwierdzenia nie może być mniejsza niż data utworzenia';
        case 'order_cost_cannot_be_negative' : 
            return 'Koszt zamówienia nie może być ujemny';
        case 'order_item_cannot_be_null' :
            return 'Pozycja zamówienia nie może być pusta';
        case 'order_item_not_found' :
            return 'Nie znaleziono pozycji zamówienia';
        case 'order_items_cannot_be_null' :
            return 'Pozycje zamówienia nie mogą być puste';
        case 'order_number_cannot_be_empty' :
            return 'Numer zamówienia nie może być pusty';
        case 'payment_number_cannot_be_empty' :
            return 'Numer płatności nie może być pusty';
        case 'currency_cannot_be_changed_in_order' :
            return 'Waluta nie może być zmieniona w zamówieniu, sprawdź status'; 
        case 'customer_cannot_be_changed_in_order' :
            return 'Dane kontaktowe nie mogą być zmienione w zamówieniu, sprawdź status';
        case 'invalid_currencies' :
            return 'Wystąpił błąd podczas dodawania przedmiotów do koszyka';
        case 'order_cannot_be_deleted' :
            return 'Zamówienie nie może zostać usunięte, sprawdź status';
        case 'order_item_cannot_be_deleted' :
            return 'Pozycja nie może zostać usunieta, sprawdź status zamówienia';
        case 'order_items_cannot_be_empty' :
            return 'Pozycje zamówienia nie mogą być puste'
        case 'order_not_found' :
            return 'Nie znaleziono zamówienia';
        case 'position_from_order_cannot_be_deleted' :
            return 'Pozycja z zamówienia nie może zostać usunięta, sprawdź status zamówienia';
        case 'position_to_order_cannot_be_added' :
            return 'Pozycja nie może zostać dodana do zamówienia, sprawdź status zamówienia';
        case 'payment_not_found':
            return 'Nie znaleziono płatności';
        default :
            return "Sprawdź poprawność podanych danych";
    }
}