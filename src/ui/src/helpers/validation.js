export function validateEmail(text) {
    const pattern = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return pattern.test(text);
}

export function validatePassword(text) {
    const pattern = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d\\w\\W]{8,}$");
    return pattern.test(text);
}

export function validate(rules = [], value) {
    for (let i = 0; i < rules.length; i++) {
        const rule = rules[i];

        if (rule instanceof Object) {
            const errorMessage = availableRules[rule.rule](value, rule);
            if (errorMessage) {
                return errorMessage;
            }
        } else {
            const errorMessage = availableRules[rule](value);
            if (errorMessage) {
                return errorMessage;
            }
        }
    }

    return '';
}

const availableRules = {
    required(value) {
        return value ? '' : 'Pole wymagane';
    },
    min(value, rule) {
      return value.length > rule.length ? '' : `Min. znaków: ${rule.length}`;
    },
    email(value) {
      return validateEmail(value) ? '' : 'Niepoprawy email';
    },
    password(value) {
        return validatePassword(value) ? '' : 'Hasło powinno zawierać przynajmniej 8 znaków, w tym jedną dużą literę i jedną liczbę';
    }
};

export function mapToMessage(code, status) {
    debugger;
    if (status >= 500) {
        return "Coś poszło nie tak";
    }

    if (status == 401) {
        return "Brak autoryzacji, spróbuj zalogować się";
    }

    if (status == 403) {
        return "Brak dostępu do zasobu";
    }

    if (status == 400) {
        switch(code) {
            case 'invalid_password' : 
                return "Niepoprawne hasło. Hasło powinno zawierać przynajmniej 8 znaków, w tym jedną dużą literę i jedną liczbę";
            case 'user_not_active' :
                return "Użytkownik nie jest aktywny";
            case 'email_in_use' :
                return "Podany email jest już używany";
            case 'invalid_credentials' :
                return "Niepoprawne dane logowania, sprawdź czy email i hasło są poprawne";
            default :
                return "Sprawdź podany url";
        }
    }
}