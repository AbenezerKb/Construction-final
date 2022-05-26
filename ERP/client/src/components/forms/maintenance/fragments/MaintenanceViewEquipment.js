import { Form, InputGroup, Table } from "react-bootstrap";
import PropTypes from "prop-types";
import { useQuery } from "react-query";
import { fetchEquipmentModel } from "../../../../api/item";

function MaintenanceViewEquipment({ addedItems, index }) {
    var modelQuery = useQuery(
        ["model", addedItems[index].equipmentModelId],
        () =>
            addedItems[index].equipmentModelId &&
            fetchEquipmentModel(addedItems[index].equipmentModelId)
    );

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
                            readOnly
                        />
                    </Form.Group>
                </div>
                <div className="col">
                    <Form.Group className="mb-3">
                        <Form.Label>Fix Remark</Form.Label>
                        <Form.Control
                            type="text"
                            name="fixRemark"
                            value={addedItems[index].fixRemark}
                            readOnly
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

MaintenanceViewEquipment.propTypes = {
    addedItems: PropTypes.array.isRequired,
    index: PropTypes.number.isRequired,
};

export default MaintenanceViewEquipment;
