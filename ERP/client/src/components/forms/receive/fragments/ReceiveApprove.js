import { Form, InputGroup } from "react-bootstrap";

function ReceiveApprove({ addedItems, setAddedItems, index }) {

    return (
        <div className="shadow pt-3 px-4 mb-4 rounded">
            <div className=" row justify-content-between">
                <div className="col-auto">
                    <h1 className="display-6 fs-4">Material {index + 1}</h1>
                </div>
            </div>

            <div className="row">
                <div className="col">
                    <Form.Group className="mb-3">
                        <Form.Label>Item</Form.Label>
                        <Form.Control
                            type="text"
                            readOnly
                            value={addedItems[index].name}
                        />
                    </Form.Group>
                </div>
                <div className="col">
                    <Form.Group className="mb-3">
                        <Form.Label>Quantity Purchased</Form.Label>
                        <InputGroup className="mb-3">
                            <Form.Control
                                type="text"
                                name="qty"
                                readOnly
                                value={addedItems[index].qtyPurchased}
                            />
                            <Form.Control
                                name="unit"
                                type="text"
                                readOnly
                                value={addedItems[index].units[0]}
                            />
                        </InputGroup>
                    </Form.Group>
                </div>
            </div>

            <div className="row">
                <div className="col">
                    <Form.Group className="mb-3">
                        <Form.Label>Specification</Form.Label>
                        <Form.Control
                            as="textarea"
                            name="spec"
                            rows={5}
                            value={addedItems[index].spec}
                            readOnly
                        />
                    </Form.Group>
                </div>
                <div className="col">
                    <div className="row">
                        <Form.Group className="mb-3">
                            <Form.Label>Quantity Received</Form.Label>
                            <Form.Control
                                type="text"
                                name="qty"
                                readOnly
                                value={addedItems[index].qtyReceived}
                            />
                        </Form.Group>
                    </div>

                    <div className="row">
                        <Form.Group className="mb-3">
                            <Form.Label>Receive Remark</Form.Label>
                            <Form.Control
                                type="text"
                                readOnly
                                value={addedItems[index].receiveRemark}
                            />
                        </Form.Group>
                    </div>
                </div>

            </div>
            
        </div>
    );
}

export default ReceiveApprove;
