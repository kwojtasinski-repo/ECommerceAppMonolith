import React, { useEffect, useRef, useState } from 'react';
import { useNavigate } from 'react-router';

function Searchbar() {
    const [term, setTerm] = useState('');
    const inputRef = useRef('');
    const navigate = useNavigate();

    const onKeyDownHandler = event => {
        if (event.key === 'Enter') {
            search();
        }
    }

    const search = () => {
        navigate(`/search/${term}`);
    }
    
    const focusInput = () => {
        inputRef.current.focus();
    }

    useEffect(() => {
        focusInput();
    }, []);

    return (
        <div className="d-flex">
            <input 
                ref={inputRef}
                value={term}
                onKeyDown={onKeyDownHandler}
                onChange={event => setTerm(event.target.value)}
                type = "text"
                className = "form-control search w-auto"
                placeholder = "Szukaj..."/>
            <button className = {`ms-1 btn btn-primary`}
                onClick={search}>Szukaj</button>
        </div>
    );
}

export default Searchbar;