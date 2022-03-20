import { Component } from "react";

class ErrorBoundary extends Component {
    state = {
        hasError: false,
        error: null
    };

    static getDerivedStateFromError(error) {
        return { hasError: true, error };
    }

    componentDidCatch(error, errorInfo) {
        console.log('Error Boundary');
        console.log(error, errorInfo);
    }

    render() {
        if (this.state.hasError) {
            return (
                <div className="alert alert-danger">
                    Wystąpił jakiś problem.
                </div>
            )
        }

        return this.props.children;
    }
}

export default ErrorBoundary;