import { Spinner } from "react-bootstrap";

function LoadingSpinner() {
    return (
        <div className="d-flex justify-content-center align-content-center mt-4 pt-4">
            <Spinner animation="grow" className="bg-teal" />
        </div>
    );
}

export default LoadingSpinner;
