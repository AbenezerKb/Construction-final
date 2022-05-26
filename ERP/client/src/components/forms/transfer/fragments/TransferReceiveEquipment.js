import { Form, InputGroup, Table } from "react-bootstrap";
import PropTypes from "prop-types";
import { useQuery } from "react-query";
import { fetchEquipmentModel } from "../../../../api/item";

function TransferReceiveMaterial({ addedItems, setAddedItems, index }) {
    var modelQuery = useQuery(
        ["model", addedItems[index].equipmentModelId],
        () =>
            addedItems[index].equipmentModelId &&
            fetchEquipmentModel(addedItems[index].equipmentModelId)
    );

    function valueChanged(e) {
        const addedItemsCpy = [...addedItems];
        addedItemsCpy[index][e.target.name] = e.target.value;
        setAddedItems(addedItemsCpy);
    }

    return (
        <div className="shadow pt-3 px-4 mb-4 rounded">
            <div className=" row justify-content-between">
                <div className="col-auto">
                    <h1 className="display-6 fs-4">Equipment {index + 1}</h1>
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
                        <Form.Label>Model</Form.Label>
                        <Form.Control
                            type="text"
                            name="cost"
                            value={modelQuery?.data?.name || ""}
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
                </div>
            </div>
            <div className="row">
                <div className="col">
                    <Form.Group className="mb-3">
                        <Form.Label>Description</Form.Label>
                        <Form.Control
                            as="textarea"
                            name="spec"
                            rows={1}
                            value={addedItems[index].description}
                            readOnly
                        />
                    </Form.Group>
                </div>
                <div className="col">
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
                        <Form.Control type="text" readOnly value={addedItems[index].sendRemark} />
                    </Form.Group>
                </div>
                <div className="col">
                    <Form.Group className="mb-3">
                        <Form.Label>Receive Remark</Form.Label>
                        <Form.Control
                            type="text"
                            name="receiveRemark"
                            value={addedItems[index].receiveRemark}
                            onChange={valueChanged}
                        />
                    </Form.Group>
                </div>
            </div>

            {addedItems[index].equipmentAssets.length ? (
                <div className="pb-1">
                    <Table striped size="sm">
                        <thead>
                            <tr>
                                <th>Asset</th>
                                <th>Asset Number</th>
                                <th>Serial Number</th>
                            </tr>
                        </thead>
                        <tbody>
                            {addedItems[index].equipmentAssets.map((asset, assetIndex) => {
                                return (
                                    <tr key={assetIndex}>
                                        <td>{assetIndex + 1}</td>
                                        <td>{asset.equipmentAsset.assetNo}</td>
                                        <td>{asset.equipmentAsset.serialNo}</td>
                                    </tr>
                                );
                            })}
                        </tbody>
                    </Table>
                </div>
            ) : (
                ""
            )}
        </div>
    );
}

TransferReceiveMaterial.propTypes = {
    addedItems: PropTypes.array.isRequired,
    setAddedItems: PropTypes.func.isRequired,
    index: PropTypes.number.isRequired,
};

export default TransferReceiveMaterial;
