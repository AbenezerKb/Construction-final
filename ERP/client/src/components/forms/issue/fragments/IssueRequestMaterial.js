import { Form, Button, InputGroup } from "react-bootstrap";
import { FaTrash } from "react-icons/fa";
import PropTypes from "prop-types";

function IssueRequestMaterial({
    allItems,
    addedItems,
    setAddedItems,
    index,
}) {
    function selectedItemChanged(e) {
        const selectedId = e.target.value;

        if (selectedId === "") return;

        const addedItemsCpy = [...addedItems];
        var item = allItems.filter((item) => item.itemId.toString() === selectedId)[0];

        addedItemsCpy[index].itemId = selectedId;
        addedItemsCpy[index].units = [item.material.unit];
        addedItemsCpy[index].spec = item.material.spec;
        addedItemsCpy[index].cost = item.material.cost;

        setAddedItems(addedItemsCpy);
    }

    function valueChanged(e) {
        const addedItemsCpy = [...addedItems];
        addedItemsCpy[index][e.target.name] = e.target.value;
        setAddedItems(addedItemsCpy);
    }

    function deleteItem() {
        const addedItemsCpy = [...addedItems];
        addedItemsCpy.splice(index, 1);
        setAddedItems(addedItemsCpy);
    }

    return (
        <div className="shadow pt-3 px-3 mb-4 rounded">
            <div className=" row justify-content-between">
                <div className="col-auto">
                    <h1 className="display-6 fs-4">Material {index + 1}</h1>
                </div>
                <div className="col-auto">
                    {!(index === 0 && addedItems.length === 1) && (
                        <Button
                            className="px-4"
                            variant="outline-danger"
                            onClick={deleteItem}
                        >
                            <FaTrash className="me-2 mb-1" />
                            Remove Item
                        </Button>
                    )}
                </div>
            </div>

            <div className="row">
                <div className="col">
                    <Form.Group className="mb-3">
                        <Form.Label>Item</Form.Label>
                        <Form.Select
                            value={addedItems[index].itemId}
                            onChange={selectedItemChanged}
                            required
                        >
                            <option value="">Select Item</option>
                            {
                                // remove an item if it is selected on another item to keep it from being added multiple times
                                allItems
                                    .filter(
                                        (currentItem) =>
                                            addedItems.filter(
                                                (addedItem) =>
                                                    addedItem.itemId ===
                                                        currentItem.itemId.toString() &&
                                                    addedItem.itemId !==
                                                        addedItems[index].itemId
                                            ).length === 0
                                    )
                                    .map((item) => (
                                        <option
                                            key={item.itemId}
                                            value={item.itemId}
                                        >
                                            {item.name}
                                        </option>
                                    ))
                            }
                        </Form.Select>
                    </Form.Group>
                </div>
                <div className="col">
                    <Form.Group className="mb-3">
                        <Form.Label>Quantity</Form.Label>
                        <InputGroup className="mb-3">
                            <Form.Control
                                min="1"
                                type="number"
                                name="qtyRequested"
                                value={addedItems[index].qtyRequested}
                                onChange={valueChanged}
                            />
                            <Form.Select
                                name="unit"
                                value={addedItems[index].unit}
                                onChange={valueChanged}
                            >
                                {addedItems[index].units.map((unit, index) => (
                                    <option key={index} value={index}>
                                        {unit}
                                    </option>
                                ))}
                            </Form.Select>
                        </InputGroup>
                    </Form.Group>
                </div>
            </div>

            <div className="row">
                <div className="col">
                    <Form.Group className="mb-3">
                        <Form.Label>Specification</Form.Label>
                        <Form.Control
                            readOnly
                            as="textarea"
                            name="spec"
                            rows={5}
                            value={addedItems[index].spec}
                        />
                    </Form.Group>
                </div>
                <div className="col">
                    <div className="row">
                        <div className="col">
                            <Form.Group className="mb-3">
                                <Form.Label>Unit Cost</Form.Label>
                                <Form.Control
                                    readOnly
                                    type="text"
                                    name="cost"
                                    value={addedItems[index].cost}
                                />
                            </Form.Group>
                        </div>
                        <div className="col">
                            <Form.Group className="mb-3">
                                <Form.Label>Total Cost</Form.Label>
                                <Form.Control
                                    readOnly
                                    type="text"
                                    name="total"
                                    value={
                                        addedItems[index].cost *
                                        addedItems[index].qtyRequested
                                    }
                                />
                            </Form.Group>
                        </div>
                    </div>
                    <Form.Group className="mb-3">
                        <Form.Label>Remark</Form.Label>
                        <Form.Control
                            type="text"
                            name="requestRemark"
                            value={addedItems[index].requestRemark}
                            onChange={valueChanged}
                        />
                    </Form.Group>
                </div>
            </div>
        </div>
    );
}

IssueRequestMaterial.propTypes = {
    allItems: PropTypes.array.isRequired,
    addedItems: PropTypes.array.isRequired,
    setAddedItems: PropTypes.func.isRequired,
    index: PropTypes.number.isRequired,
};

export default IssueRequestMaterial;
