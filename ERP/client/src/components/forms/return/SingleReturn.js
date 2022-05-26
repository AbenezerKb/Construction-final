import { useEffect, useState, useMemo } from "react";
import { useParams } from "react-router-dom";
import { Form, Container, Alert } from "react-bootstrap";
import Item from "../../../models/Item";
import Header from "../../layouts/Header";
import ViewReturn from "./ViewReturn.js";
import { useQuery } from "react-query";
import LoadingSpinner from "../../fragments/LoadingSpinner";
import ConnectionError from "../../fragments/ConnectionError";
import { fetchReturn } from "../../../api/return";

function SingleReturn() {
    const { id } = useParams();
    const [thisReturn, setThisReturn] = useState({});
    const [addedItems, setAddedItems] = useState([]);
    const [isLoading, setIsLoading] = useState(true);

    var query = useQuery(["return", id], () => fetchReturn(id));

    useEffect(() => {
        if (query.data === undefined) return;
        setThisReturn(query.data);
        let items = [];

        for (var returnItem of query.data.returnEquipmentAssets) {
            let itemObj = new Item();

            itemObj.itemId = returnItem.ItemId;
            itemObj.equipmentModelId = returnItem.equipmentModelId;
            itemObj.equipmentAssetId = returnItem.equipmentAssetId;
            itemObj.returnRemark = returnItem.returnRemark;
            itemObj.description = returnItem.equipmentAsset.equipmentModel.equipment.description;
            itemObj.assetDamageId = returnItem.assetDamageId;
            itemObj.assetDamage = returnItem.assetDamage?.name ?? "No Damage";
            itemObj.fileName = returnItem.fileName;
            itemObj.name = returnItem.equipmentAsset.equipmentModel.equipment.item.name;

            itemObj.requestedBy = returnItem.borrow.requestedBy;
            itemObj.handDate = returnItem.borrow.handDate;

            itemObj.equipmentAssets = [
                {
                    equipmentAssetId: returnItem.equipmentAsset.equipmentAssetId,
                    assetNo: returnItem.equipmentAsset.assetNo,
                    serialNo: returnItem.equipmentAsset.serialNo,
                },
            ];

            items.push(itemObj);
        }

        setAddedItems(items);
        setIsLoading(false);
    }, [query.data]);

    function FormRow({ labelL, labelR, valueL, valueR }) {
        //defines layout of each row
        return (
            <div className="row mx-2">
                <div className="col">
                    <Form.Group className="mb-3">
                        <div className="row">
                            <div className="col-3 mt-1">
                                <Form.Label>{labelL}</Form.Label>
                            </div>
                            <div className="col">
                                {typeof valueL === "object" ? (
                                    valueL
                                ) : (
                                    <Form.Control type="text" readOnly value={valueL} />
                                )}
                            </div>
                        </div>
                    </Form.Group>
                </div>
                <div className="col">
                    <Form.Group className="mb-3">
                        <div className="row">
                            <div className="col-3 mt-1">
                                <Form.Label>{labelR}</Form.Label>
                            </div>
                            <div className="col">
                                {typeof valueR === "object" ? (
                                    valueR
                                ) : (
                                    <Form.Control type="text" readOnly value={valueR} />
                                )}
                            </div>
                        </div>
                    </Form.Group>
                </div>
            </div>
        );
    }

    function TopForm() {
        return (
            <Form>
                <FormRow
                    labelL="Return Number"
                    valueL={thisReturn.returnId}
                    labelR="Return Site"
                    valueR={thisReturn.site.name}
                />
                <FormRow
                    labelL="Returned By"
                    valueL={`${thisReturn.returnedBy.fName} ${thisReturn.returnedBy.mName}`}
                    labelR="Returned On"
                    valueR={new Date(thisReturn.returnDate).toLocaleString()}
                />
            </Form>
        );
    }

    if (isLoading) return <LoadingSpinner />;

    if (query.isError) return <ConnectionError status={query?.error?.response?.status} />;

    return (
        <>
            <Header title={"Returned Assets"} showPrint/>

            <Container className="my-3">
                <TopForm />
                <ViewReturn addedItems={addedItems} />
            </Container>
        </>
    );
}

export default SingleReturn;
