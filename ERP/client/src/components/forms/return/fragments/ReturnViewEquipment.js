import { Form, InputGroup, Table, Card } from "react-bootstrap";
import PropTypes from "prop-types";
import { useQuery } from "react-query";
import { fetchEquipmentModel } from "../../../../api/item";
import { FaFolderOpen, FaDownload } from "react-icons/fa";
import { url as host } from "../../../../api/api";

function ReturnViewEquipment({ addedItems, index }) {
    var modelQuery = useQuery(
        ["model", addedItems[index].equipmentModelId],
        () =>
            addedItems[index].equipmentModelId &&
            fetchEquipmentModel(addedItems[index].equipmentModelId)
    );

    return (
        <>
            <div className="shadow pt-3 px-4 mb-4 rounded d-print-none">
                <div className=" row justify-content-between">
                    <div className="col-auto">
                        <h1 className="display-6 fs-4">Equipment {index + 1}</h1>
                    </div>
                </div>

                <div className="row">
                    <div className="col">
                        <Form.Group className="mb-3">
                            <Form.Label>Borrowed By</Form.Label>
                            <Form.Control
                                type="text"
                                value={`${addedItems[index].requestedBy.fName} ${addedItems[index].requestedBy.mName}`}
                                readOnly
                            />
                        </Form.Group>
                    </div>
                    <div className="col">
                        <Form.Group className="mb-3">
                            <Form.Label>Handed Over On</Form.Label>
                            <Form.Control
                                type="text"
                                value={new Date(addedItems[index].handDate).toLocaleString()}
                                readOnly
                            />
                        </Form.Group>
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
                                value={modelQuery?.data?.name || ""}
                                readOnly
                            />
                        </Form.Group>
                    </div>
                </div>
                <div className="row">
                    <div className="col">
                        <Form.Group className="mb-3">
                            <Form.Label>Asset Number</Form.Label>
                            <Form.Control
                                type="text"
                                value={addedItems[index].equipmentAssets[0].assetNo}
                                readOnly
                            />
                        </Form.Group>
                    </div>
                    <div className="col">
                        <Form.Group className="mb-3">
                            <Form.Label>Serial Number</Form.Label>
                            <Form.Control
                                type="text"
                                value={addedItems[index].equipmentAssets[0].serialNo}
                                readOnly
                            />
                        </Form.Group>
                    </div>
                </div>

                <div className="row">
                    <div className="col">
                        <Form.Group className="mb-3">
                            <Form.Label>Damage</Form.Label>
                            <Form.Control
                                type="text"
                                value={addedItems[index].assetDamage}
                                readOnly
                            />
                        </Form.Group>
                    </div>
                    <div className="col">
                        <Form.Group className="mb-3">
                            <Form.Label>Return Remark</Form.Label>
                            <Form.Control
                                type="text"
                                value={addedItems[index].returnRemark}
                                readOnly
                            />
                        </Form.Group>
                    </div>
                </div>

                {addedItems[index].fileName ? (
                    <div className="row d-flex flex-row justify-content-between pb-3">
                        <div className="col">
                            <Form.Label className="mt-2 ms-3">Asset Damage Attachment</Form.Label>
                        </div>
                        <div className="col d-grid">
                            <a
                                className="btn btn-teal"
                                href={`${host}file/${addedItems[index].fileName}`}
                                target="_blank"
                            >
                                <FaFolderOpen className="me-2 mb-1" />
                                Open
                            </a>
                        </div>
                        <div className="col d-grid">
                            <a
                                className="btn btn-teal mx-2"
                                href={`${host}file/download/${addedItems[index].fileName}`}
                            >
                                <FaDownload className="me-2 mb-1" />
                                Download
                            </a>
                        </div>
                    </div>
                ) : null}
            </div>

            <tr className="d-none d-print-table-row">
                <th>{index + 1}</th>
                <td>{addedItems[index].name}</td>
                <td>{modelQuery?.data?.name || ""}</td>
                <td>{`${addedItems[index].requestedBy.fName} ${addedItems[index].requestedBy.mName}`}</td>
                <td>{new Date(addedItems[index].handDate).toLocaleString()}</td>
                <td>{addedItems[index].equipmentAssets[0].assetNo}</td>
                <td>{addedItems[index].equipmentAssets[0].serialNo}</td>
                <td>{addedItems[index].assetDamage}</td>
                <td>{addedItems[index].returnRemark}</td>
            </tr>
        </>
    );
}

ReturnViewEquipment.propTypes = {
    addedItems: PropTypes.array.isRequired,
    index: PropTypes.number.isRequired,
};

export default ReturnViewEquipment;
