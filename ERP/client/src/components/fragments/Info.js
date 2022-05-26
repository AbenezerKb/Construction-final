import { useState } from "react";
import { Toast } from "react-bootstrap";

function Info() {
    const delay = 5000;
    const [showA, setShowA] = useState(true);

    const toggleShowA = () => setShowA(!showA);

    return (
        <div className="position-fixed top-0 end-0 p-3">
            <Toast show={showA} onClose={toggleShowA} delay={delay} autohide>
                <Toast.Header>
                    <strong className="me-auto">Transfer Out</strong>
                </Toast.Header>
                <Toast.Body>Items Sent</Toast.Body>
            </Toast>
        </div>
    );
}

export default Info;
