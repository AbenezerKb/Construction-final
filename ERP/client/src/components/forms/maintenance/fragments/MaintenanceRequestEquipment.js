import { useState, useEffect } from "react";
import PropTypes from "prop-types";
import { Form, Button, InputGroup } from "react-bootstrap";
import { FaTrash } from "react-icons/fa";
import { useQuery } from "react-query";
import { fetchItem, fetchEquipmentModel } from "../../../../api/item";
import { fetchDamagedAssets } from "../../../../api/maintenance";
import { fetchEquipmentCategories, fetchEquipmentCategory } from "../../../../api/category";
import ConnectionError from "../../../fragments/ConnectionError";

function MaintenanceRequestEquipment({ addedItems, setAddedItems, index }) {
    const [categoryId, setCategoryId] = useState(0);
    const [itemId, setItemId] = useState(0);
    const [modelId, setModelId] = useState(0);

    const categoriesQuery = useQuery("equipmentcategories", fetchEquipmentCategories);

    var itemsQuery = useQuery(
        ["equipmentcategory", categoryId],
        () => categoryId && fetchEquipmentCategory(categoryId)
    );
    var itemQuery = useQuery(["item", itemId], () => itemId && fetchItem(itemId));
    var modelQuery = useQuery(["model", modelId], () => modelId && fetchEquipmentModel(modelId));

    var assetsQuery = useQuery(["assets", modelId], () => modelId && fetchDamagedAssets(modelId));

    useEffect(() => {
        //Initialize asset number entry array
        //it is set to 1 here
        const addedItemsCpy = [...addedItems];
        addedItemsCpy[index].qtyRequested = 1;
        setAddedItems(addedItemsCpy);
    }, []);

    useEffect(() => {
        const addedItemsCpy = [...addedItems];

        const extraItems =
            addedItems[index].qtyRequested - addedItems[index].equipmentAssets.length;

        if (extraItems > 0)
            addedItemsCpy[index].equipmentAssets.push(
                ...Array(extraItems).fill({ equipmentAssetId: "" })
            );

        if (extraItems < 0) addedItemsCpy[index].equipmentAssets.splice(extraItems);

        setAddedItems(addedItemsCpy);
    }, [addedItems[index].qtyRequested]);

    useEffect(() => {
        if (!modelQuery.data) return;

        const addedItemsCpy = [...addedItems];
        addedItemsCpy[index].itemId = itemId;
        addedItemsCpy[index].equipmentModelId = modelId;

        addedItemsCpy[index].units = [itemQuery.data.equipment.unit];
        addedItemsCpy[index].description = itemQuery.data.equipment.description;

        addedItemsCpy[index].cost = modelQuery.data.cost;

        setAddedItems(addedItemsCpy);
    }, [modelQuery.data]);

    function valueChanged(e) {
        const addedItemsCpy = [...addedItems];
        addedItemsCpy[index][e.target.name] = e.target.value;
        setAddedItems(addedItemsCpy);
    }

    function assetChanged(changedIndex, e) {
        const newAddedItemsCpy = addedItems.map((item, i) => {
            if (i !== index) return item;

            let { equipmentAssets } = item;

            const newEquipmentAssets = equipmentAssets.map((asset, assetIndex) => {
                if (assetIndex !== changedIndex) return asset;
                return { equipmentAssetId: e.target.value };
            });

            return { ...item, equipmentAssets: newEquipmentAssets };
        });

        setAddedItems(newAddedItemsCpy);
    }

    function deleteItem() {
        const addedItemsCpy = [...addedItems];
        addedItemsCpy.splice(index, 1);
        setAddedItems(addedItemsCpy);
    }

    if (categoriesQuery.isError || itemsQuery.isError || itemQuery.isError || modelQuery.isError)
        return (
            <ConnectionError
                status={
                    categoriesQuery?.error?.response?.status ??
                    itemsQuery?.error?.response?.status ??
                    itemQuery?.error?.response?.status ??
                    modelQuery?.error?.response?.status
                }
            />
        );

    return (
        <div className="shadow pt-3 px-3 mb-4 rounded">
            <div className=" row justify-content-between">
                <div className="col-auto">
                    <h1 className="display-6 fs-4">Equipment {index + 1}</h1>
                </div>
                <div className="col-auto">
                    {!(index === 0 && addedItems.length === 1) && (
                        <Button className="px-4" variant="outline-danger" onClick={deleteItem}>
                            <FaTrash className="me-2 mb-1" />
                            Remove Item
                        </Button>
                    )}
                </div>
            </div>

            <div className="row">
                <div className="col">
                    <Form.Group className="mb-3">
                        <Form.Label>Category</Form.Label>
                        <Form.Select
                            value={categoryId}
                            onChange={(e) => setCategoryId(e.target.value)}
                            required
                        >
                            <option value="">Select Equipment Category</option>
                            {categoriesQuery.data?.map((category) => (
                                <option
                                    key={category.equipmentCategoryId}
                                    value={category.equipmentCategoryId}
                                >
                                    {category.name}
                                </option>
                            ))}
                        </Form.Select>
                    </Form.Group>
                </div>
                <div className="col">
                    <Form.Group className="mb-3">
                        <Form.Label>Item</Form.Label>
                        <Form.Select
                            value={itemId}
                            onChange={(e) => setItemId(e.target.value)}
                            required
                        >
                            <option value="">Select Equipment</option>
                            {itemsQuery.data?.equipments?.map((equipment) => (
                                <option key={equipment.itemId} value={equipment.itemId}>
                                    {equipment.item.name}
                                </option>
                            ))}
                        </Form.Select>
                    </Form.Group>
                </div>
            </div>

            <div className="row">
                <div className="col">
                    <Form.Group className="mb-3">
                        <Form.Label>Model</Form.Label>
                        <Form.Select
                            value={modelId}
                            onChange={(e) => setModelId(e.target.value)}
                            required
                        >
                            <option value="">Select Model</option>
                            {itemQuery.data?.equipment?.equipmentModels
                                ?.filter(
                                    (currentItem) =>
                                        addedItems.filter(
                                            (addedItem) =>
                                                addedItem.equipmentModelId ===
                                                    currentItem.equipmentModelId.toString() &&
                                                addedItem.equipmentModelId !==
                                                    addedItems[index].equipmentModelId
                                        ).length === 0
                                )
                                .map((equipmentModel) => (
                                    <option
                                        key={equipmentModel.equipmentModelId}
                                        value={equipmentModel.equipmentModelId}
                                    >
                                        {equipmentModel.name}
                                    </option>
                                ))}
                        </Form.Select>
                    </Form.Group>
                </div>
                <div className="col">
                    <div className="row">
                        <div className="col">
                            <Form.Group className="mb-3">
                                <Form.Label>Equipment Cost</Form.Label>
                                <Form.Control
                                    readOnly
                                    type="text"
                                    name="cost"
                                    value={addedItems[index].cost}
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
                            readOnly
                            as="textarea"
                            name="description"
                            rows={3}
                            value={addedItems[index].description}
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
                            rows={3}
                            value={addedItems[index].reason}
                            onChange={valueChanged}
                        />
                    </Form.Group>
                </div>
            </div>

            {addedItems[index].equipmentAssets.map((asset, assetIndex) => {
                return (
                    <div className="row" key={assetIndex}>
                        <div className="col">
                            <Form.Group className="mb-3">
                                <Form.Label> Asset Number</Form.Label>
                                <Form.Select
                                    value={asset.equipmentAssetId}
                                    name={assetIndex}
                                    onChange={(e) => assetChanged(assetIndex, e)}
                                    required
                                >
                                    <option value="">Select Asset Number</option>
                                    {assetsQuery.data &&
                                        assetsQuery.data
                                            ?.filter(
                                                (currentItem) =>
                                                    addedItems[index].equipmentAssets.filter(
                                                        (addedItem) =>
                                                            addedItem.equipmentAssetId ===
                                                                currentItem.equipmentAssetId.toString() &&
                                                            addedItem.equipmentAssetId !==
                                                                addedItems[index].equipmentAssets[
                                                                    assetIndex
                                                                ].equipmentAssetId
                                                    ).length === 0
                                            )
                                            ?.map((equipmentAsset) => (
                                                <option
                                                    key={equipmentAsset.equipmentAssetId}
                                                    value={equipmentAsset.equipmentAssetId}
                                                >
                                                    {equipmentAsset.assetNo}
                                                </option>
                                            ))}
                                </Form.Select>
                            </Form.Group>
                        </div>
                        <div className="col">
                            <Form.Group className="mb-3">
                                <Form.Label>Serial Number</Form.Label>
                                <Form.Control
                                    type="text"
                                    value={
                                        modelQuery.data?.equipmentAssets?.find(
                                            (e) =>
                                                e.equipmentAssetId.toString() ===
                                                asset.equipmentAssetId
                                        )?.serialNo || ""
                                    }
                                    readOnly
                                />
                            </Form.Group>
                        </div>
                    </div>
                );
            })}
        </div>
    );
}

MaintenanceRequestEquipment.propTypes = {
    addedItems: PropTypes.array.isRequired,
    setAddedItems: PropTypes.func.isRequired,
    index: PropTypes.number.isRequired,
};

export default MaintenanceRequestEquipment;
