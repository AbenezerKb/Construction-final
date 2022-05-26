import { useState, useEffect, useRef } from "react";
import { useQuery } from "react-query";
import { fetchEquipmentModel, fetchCleanEquipmentAssets } from "../../../../api/item";
import { Form, InputGroup, Row, Col } from "react-bootstrap";
import PropTypes from "prop-types";
import Select from "react-select";

function BorrowHandEquipment({ addedItems, setAddedItems, index }) {
    const [assetNumbers, setAssetNumbers] = useState([]);
    const assetSelectedNumberRef = useRef();

    var modelQuery = useQuery(
        ["model", addedItems[index].equipmentModelId],
        () =>
            addedItems[index].equipmentModelId &&
            fetchEquipmentModel(addedItems[index].equipmentModelId)
    );

    var assetsQuery = useQuery(
        ["assetsclean", addedItems[index].equipmentModelId],
        () =>
            addedItems[index].equipmentModelId &&
            fetchCleanEquipmentAssets(addedItems[index].equipmentModelId)
    );

    useEffect(() => {
        if (!assetsQuery?.data) return setAssetNumbers([]);

        var data = [];

        for (var equipmentAsset of assetsQuery?.data) {
            data.push({
                value: equipmentAsset.equipmentAssetId,
                label: equipmentAsset.assetNo,
            });
        }

        setAssetNumbers(data);
    }, [assetsQuery.data]);

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
                        <Form.Label>Quantity Approved</Form.Label>
                        <InputGroup className="mb-3">
                            <Form.Control
                                type="text"
                                name="qty"
                                readOnly
                                value={addedItems[index].qtyApproved}
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
                                <Form.Label>Total Cost</Form.Label>
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
                        <Form.Label>Hand Over Remark</Form.Label>
                        <Form.Control
                            type="text"
                            name="handRemark"
                            value={addedItems[index].handRemark}
                            onChange={valueChanged}
                        />
                    </Form.Group>
                </div>
            </div>

            <div>
                <Row>
                    <Col>
                        <Form.Group className="mb-3">
                            <Form.Label>Asset Numbers</Form.Label>
                            <Select
                                placeholder="Select Asset Numbers"
                                style={{ display: "inline" }}
                                value={addedItems[index].equipmentAssets}
                                onChange={(o) =>
                                    valueChanged({ target: { name: "equipmentAssets", value: o } })
                                }
                                className="asset-select"
                                closeMenuOnSelect={false}
                                isOptionDisabled={() =>
                                    addedItems[index].equipmentAssets.length >=
                                    addedItems[index].qtyApproved
                                }
                                isMulti
                                options={assetNumbers}
                            />
                        </Form.Group>
                    </Col>
                    <Col xs="auto">
                        <Form.Group className="mb-3">
                            <Form.Label>Remaining</Form.Label>
                            <Form.Control
                                className="asset-select-number"
                                style={{ width: "5rem" }}
                                onInvalid={(e) =>
                                    e.target.setCustomValidity(
                                        `Please Select ${e.target.value} More Asset${
                                            e.target.value > 1 ? "s" : ""
                                        }`
                                    )
                                }
                                type="number"
                                min={0}
                                max={0}
                                value={
                                    addedItems[index].qtyApproved -
                                    addedItems[index].equipmentAssets.length
                                }
                                ref={assetSelectedNumberRef}
                                onChange={() => {}}
                            />
                        </Form.Group>
                    </Col>
                </Row>
            </div>
        </div>
    );
}

BorrowHandEquipment.propTypes = {
    addedItems: PropTypes.array.isRequired,
    setAddedItems: PropTypes.func.isRequired,
    index: PropTypes.number.isRequired,
};

export default BorrowHandEquipment;
