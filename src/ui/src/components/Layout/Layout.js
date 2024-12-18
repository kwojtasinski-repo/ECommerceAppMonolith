function Layout(props) {
    return (
        <>
            <div>{props.header}</div>
            <div className="container">{props.menu}</div>
            <div className="container">{props.content}</div>
            <div className="py-5">{props.footer}</div>
        </>
    );
}

export default Layout;