import { useEffect } from "react";
import { Form, InputGroup, Table } from "react-bootstrap";
import { useQuery } from "react-query";
import { fetchEquipmentModel } from "../../../../api/item";

function MaintenanceApproveEquipment({ addedItems, setAddedItems, index }) {
    var modelQuery = useQuery(
        ["model", addedItems[index].equipmentModelId],
        () =>
            addedItems[index].equipmentModelId &&
            fetchEquipmentModel(addedItems[index].equipmentModelId)
    );

    //set approved qty to requested as default
    useEffect(() => {
        const addedItemsCpy = [...addedItems];
        addedItemsCpy[index].qtyApproved = addedItemsCpy[index].qtyRequested;
        setAddedItems(addedItemsCpy);
    }, []);

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
                        <Form.Label>Model</Form.Label>
                        <Form.Control
                            type="text"
                            name="cost"
                            value={modelQuery?.data?.name || ""}
                            readOnly
                        />
                    </Form.Group>
                </div>
            </div>
            <div className="row">
                <div className="col">
                <Form.Group className="mb-3">
                        <Form.Label>Equipment Cost</Form.Label>
                        <Form.Control
                            type="text"
                            name="cost"
                            value={addedItems[index].cost}
                            readOnly
                        />
                    </Form.Group>
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
                    <Form.Group className="mb-3">
                        <Form.Label>Reason of Damage</Form.Label>
                        <Form.Control
                            required
                            as="textarea"
                            name="reason"
                            rows={5}
                            value={addedItems[index].reason}
                            readOnly
                        />
                    </Form.Group>
                </div>
            </div>
            <div className="row">
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

            {addedItems[index].equipmentAssets.length && addedItems[index].equipmentAssets[0] ? (
                <div className="pb-1">
                    <Table striped size="sm">
                        <thead>
                            <tr>
                                <th>#</th>
                                <th>Asset Number</th>
                                <th>Serial Number</th>
                            </tr>
                        </thead>
                        <tbody>
                            {addedItems[index].equipmentAssets.map((asset, assetIndex) => {
                                return (
                                    <tr key={assetIndex}>
                                        <td>{assetIndex + 1}</td>
                                        <td>{asset.assetNo}</td>
                                        <td>{asset.serialNo}</td>
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

export default MaintenanceApproveEquipment;
