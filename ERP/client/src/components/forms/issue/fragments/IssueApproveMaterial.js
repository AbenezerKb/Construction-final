import { useEffect } from "react";
import { Form, InputGroup } from "react-bootstrap";
import { useQuery } from "react-query";
import { fetchQty } from "../../../../api/item";

function IssueApproveMaterial({ addedItems, setAddedItems, index }) {
    const qtyQuery = useQuery(["itemQty", addedItems[index].itemId], () =>
        fetchQty({ itemId: addedItems[index].itemId })
    );
    //set approved qty to requested as default
    useEffect(() => {
        if (!qtyQuery.data) return;

        const addedItemsCpy = [...addedItems];
        addedItemsCpy[index].qtyApproved = Math.min(addedItems[index].qtyRequested, qtyQuery.data);
        setAddedItems(addedItemsCpy);
    }, [qtyQuery.data]);

    function valueChanged(e) {
        const addedItemsCpy = [...addedItems];
        addedItemsCpy[index][e.target.name] = e.target.value;
        setAddedItems(addedItemsCpy);
    }

    return (
        <div className="shadow pt-3 px-4 mb-4 rounded">
            <div className=" row justify-content-between">
                <div className="col d-flex justify-content-between">
                    <span className="display-6 fs-4">Material {index + 1}</span>
                    <span className="display-6 fs-4">
                        Available Qty: <strong>{qtyQuery.data}</strong>
                    </span>
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
                        <Form.Label>Quantity Issued</Form.Label>
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
                                    value={addedItems[index].qtyRequested * addedItems[index].cost}
                                />
                            </Form.Group>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col">
                            <Form.Group className="mb-3">
                                <Form.Label>Quantity Approved</Form.Label>
                                <Form.Control
                                    min="0"
                                    max={Math.min(
                                        addedItems[index].qtyRequested,
                                        qtyQuery.data ?? 0
                                    )}
                                    type="number"
                                    name="qtyApproved"
                                    value={addedItems[index].qtyApproved}
                                    onChange={(e) => {
                                        if (e.target.value <= e.target.max) valueChanged(e);
                                    }}
                                    required
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
                                    value={addedItems[index].qtyApproved * addedItems[index].cost}
                                />
                            </Form.Group>
                        </div>
                    </div>
                </div>
            </div>
            <div className="row">
                <div className="col">
                    <Form.Group className="mb-3">
                        <Form.Label>Issue Request Remark</Form.Label>
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
                            onChange={valueChanged}
                        />
                    </Form.Group>
                </div>
            </div>
        </div>
    );
}

export default IssueApproveMaterial;
