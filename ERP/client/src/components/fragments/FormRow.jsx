import { Form } from "react-bootstrap";
function FormRow({labelL, valueL, labelR, valueR}) {
    return (
        <div className="row mx-2">
            <div className="col">
                <Form.Group className="mb-3">
                    <div className="row">
                        <div className="col-3 mt-1">
                            <Form.Label>{labelL}</Form.Label>
                        </div>
                        <div className="col">
                            {typeof valueL === "object" ? (
                                valueL
                            ) : (
                                <Form.Control type="text" readOnly value={valueL} />
                            )}
                        </div>
                    </div>
                </Form.Group>
            </div>
            <div className="col">
                <Form.Group className="mb-3">
                    <div className="row">
                        <div className="col-3 mt-1">
                            <Form.Label>{labelR}</Form.Label>
                        </div>
                        <div className="col">
                            {typeof valueR === "object" ? (
                                valueR
                            ) : (
                                <Form.Control type="text" readOnly value={valueR} />
                            )}
                        </div>
                    </div>
                </Form.Group>
            </div>
        </div>
    );
}

export default FormRow;
