import { Form, InputGroup } from "react-bootstrap";
import PropTypes from "prop-types";

function TransferViewMaterial({ addedItems, index }) {
    return (
        <>
            <div className="shadow pt-3 px-4 mb-4 rounded d-print-none">
                <div className=" row justify-content-between">
                    <div className="col-auto">
                        <h1 className="display-6 fs-4">Material {index + 1}</h1>
                    </div>
                </div>

                <div className="row">
                    <div className="col">
                        <Form.Group className="mb-3">
                            <Form.Label>Item</Form.Label>
                            <Form.Control type="text" readOnly value={addedItems[index].name} />
                        </Form.Group>
                    </div>
                    <div className="col">
                        <Form.Group className="mb-3">
                            <Form.Label>Quantity Requested</Form.Label>
                            <InputGroup className="mb-3">
                                <Form.Control
                                    type="text"
                                    name="qty"
                                    readOnly
                                    value={addedItems[index].qtyRequested}
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
                            <div className="col">
                                <Form.Group className="mb-3">
                                    <Form.Label>Unit Cost</Form.Label>
                                    <Form.Control
                                        type="text"
                                        name="cost"
                                        value={addedItems[index].cost}
                                        readOnly
                                    />
                                </Form.Group>
                            </div>
                            <div className="col">
                                <Form.Group className="mb-3">
                                    <Form.Label>Requested Total Cost</Form.Label>
                                    <Form.Control
                                        type="text"
                                        name="total"
                                        readOnly
                                        value={
                                            addedItems[index].qtyRequested * addedItems[index].cost
                                        }
                                    />
                                </Form.Group>
                            </div>
                        </div>
                        <div className="row">
                            <div className="col">
                                <Form.Group className="mb-3">
                                    <Form.Label>Quantity Approved</Form.Label>
                                    <Form.Control
                                        type="text"
                                        name="qtySent"
                                        value={addedItems[index].qtyApproved}
                                        readOnly
                                    />
                                </Form.Group>
                            </div>
                            <div className="col">
                                <Form.Group className="mb-3">
                                    <Form.Label>Approved Total Cost</Form.Label>
                                    <Form.Control
                                        type="text"
                                        name="total"
                                        readOnly
                                        value={
                                            addedItems[index].qtyApproved * addedItems[index].cost
                                        }
                                    />
                                </Form.Group>
                            </div>
                        </div>
                    </div>
                </div>

                <div className="row">
                    <div className="col">
                        <Form.Group className="mb-3">
                            <Form.Label>Request Remark</Form.Label>
                            <Form.Control
                                type="text"
                                readOnly
                                value={addedItems[index].requestRemark}
                            />
                        </Form.Group>
                    </div>
                    <div className="col">
                        <Form.Group className="mb-3">
                            <Form.Label>Approve Remark</Form.Label>
                            <Form.Control
                                type="text"
                                name="approveRemark"
                                value={addedItems[index].approveRemark}
                                readOnly
                            />
                        </Form.Group>
                    </div>
                </div>

                <div className="row">
                    <div className="col">
                        <Form.Group className="mb-3">
                            <Form.Label>Send Remark</Form.Label>
                            <Form.Control
                                type="text"
                                readOnly
                                value={addedItems[index].sendRemark}
                            />
                        </Form.Group>
                    </div>
                    <div className="col">
                        <Form.Group className="mb-3">
                            <Form.Label>Receive Remark</Form.Label>
                            <Form.Control
                                type="text"
                                name="receiveRemark"
                                value={addedItems[index].receiveRemark}
                                readOnly
                            />
                        </Form.Group>
                    </div>
                </div>
            </div>

            <tr className="d-none d-print-table-row">
                <th>{index + 1}</th>
                <td>{addedItems[index].name}</td>
                <td>{addedItems[index].spec}</td>
                <td>{addedItems[index].qtyRequested}</td>
                <td>{addedItems[index].qtyApproved}</td>
                <td>{addedItems[index].cost}</td>
                <td>{addedItems[index].qtyApproved * addedItems[index].cost}</td>
                <td>{addedItems[index].requestRemark}</td>
                <td>{addedItems[index].approveRemark}</td>
                <td>{addedItems[index].sendRemark}</td>
                <td>{addedItems[index].receiveRemark}</td>
            </tr>
        </>
    );
}

TransferViewMaterial.propTypes = {
    addedItems: PropTypes.array.isRequired,
    index: PropTypes.number.isRequired,
};

export default TransferViewMaterial;
