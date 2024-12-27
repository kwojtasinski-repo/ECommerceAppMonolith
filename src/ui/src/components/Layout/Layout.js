import { useContext } from "react";
import AuthContext from "../../context/AuthContext";

function Layout(props) {
    const authContext = useContext(AuthContext);

    return (
        <>
            <div>{props.header}</div>
            <div className="container">{props.menu}</div>
            <div className="container">
                {authContext.intializing ?
                    (
                    <p>Ładowanie...</p>
                    )
                    :
                    (
                        <>
                            {props.content}
                        </>
                    )
                }
            </div>
            <div className="py-5">{props.footer}</div>
        </>
    );
}

export default Layout;