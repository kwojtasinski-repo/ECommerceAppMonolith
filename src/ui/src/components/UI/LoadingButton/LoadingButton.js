export default function LoadingButton(props) {
    const buttonProps = {...props};
    delete buttonProps.loading;

    return (props.loading ?
        (
            <button className={`btn btn-success`} disabled >
                <span className="spinner-border spinner-border-sm" role="status"></span>
                <span className="sr-only">Ładowanie...</span>
            </button>
        )
        : <button {...buttonProps} className={`btn btn-success`}>{props.children}</button>
    )
}