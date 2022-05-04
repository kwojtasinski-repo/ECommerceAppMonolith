import { mapCodeToMessage } from "./errorCodeMapper";
import { isEmpty } from "./stringExtensions"

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
      return value.length >= rule.length ? '' : `Min. znaków: ${rule.length}`;
    },
    email(value) {
      return validateEmail(value) ? '' : 'Niepoprawy email';
    },
    password(value) {
        return validatePassword(value) ? '' : 'Hasło powinno zawierać przynajmniej 8 znaków, w tym jedną dużą literę i jedną liczbę';
    },
    only(value, rule) {
        return value.length === rule.length ? '' : `Pole powinno zawierać znaków ${rule.length}`;
    },
    requiredIf(value, rules) {
        if (rules.isRequired) {
            let errorMessage = '';
            
            if (isEmpty(value)) {
                errorMessage = 'Pole wymagane';
                return errorMessage;
            }

            if (rules.rules) {
                const rulesInside = rules.rules;
                for(const key in rulesInside) {
                    const rule = rulesInside[key];
                    errorMessage = availableRules[rule.rule](value, rule);

                    if (errorMessage) {
                        return errorMessage;
                    }
                }
            }
        }
    }
};

export function mapToMessage(code, status) {
    if (status >= 500) {
        return "Coś poszło nie tak";
    }

    if (status == 401) {
        return "Brak autoryzacji, spróbuj zalogować się";
    }

    if (status == 403) {
        return "Brak dostępu do zasobu";
    }

    if (status == 404) {
        return "Sprawdź podany url";
    }

    if (status == 400) {
        return mapCodeToMessage(code);
    }
}