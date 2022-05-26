import { useEffect, useState, useMemo } from "react";
import { useParams } from "react-router-dom";
import { Form, Container } from "react-bootstrap";
import Item from "../../../models/Item";
import Header from "../../layouts/Header";
import TransferStatusBadge from "./fragments/TransferStatusBadge";
import { ITEMTYPE, TRANSFERSTATUS } from "../../../Constants";
import ApproveTransfer from "./ApproveTransfer";
import SendTransfer from "./SendTransfer";
import ReceiveTransfer from "./ReceiveTransfer";
import ViewTransfer from "./ViewTransfer";
import DeclinedTransfer from "./DeclinedTransfer";
import { useQuery } from "react-query";
import FormRow from "../../fragments/FormRow";
import { fetchTransfer } from "../../../api/transfer";
import LoadingSpinner from "../../fragments/LoadingSpinner";
import ConnectionError from "../../fragments/ConnectionError";
import { useAuth } from "../../../contexts/AuthContext";
import AlertNotification from "../../fragments/AlertNotification";

function SingleTransfer() {
    const { id } = useParams();
    const [transfer, setTransfer] = useState({});
    const [addedItems, setAddedItems] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [viewOnly, setViewOnly] = useState();
    const [notificationType, setNotificationType] = useState(null);
    const auth = useAuth();

    var query = useQuery(["transfer", id], () => fetchTransfer(id));

    useEffect(() => {
        if (query.data === undefined) return;
        setTransfer(query.data);
        let items = [];
        for (let transferItem of query.data.transferItems) {
            let itemObj = new Item();

            itemObj.itemId = transferItem.itemId;
            itemObj.name = transferItem.item.name;
            itemObj.cost = transferItem.cost;
            itemObj.type = transferItem.item.type;

            itemObj.qtyRequested = transferItem.qtyRequested;
            itemObj.requestRemark = transferItem.requestRemark;

            if (
                query.data.status >= TRANSFERSTATUS.APPROVED ||
                query.data.status === TRANSFERSTATUS.DECLINED
            ) {
                itemObj.qtyApproved = transferItem.qtyApproved;
                itemObj.approveRemark = transferItem.approveRemark;
            }

            if (query.data.status >= TRANSFERSTATUS.SENT) {
                itemObj.sendRemark = transferItem.sendRemark;
            }

            if (query.data.status >= TRANSFERSTATUS.RECEIVED) {
                itemObj.receiveRemark = transferItem.receiveRemark;
            }

            if (transferItem.item.type === ITEMTYPE.MATERIAL) {
                itemObj.spec = transferItem.item.material.spec;
                itemObj.units = [transferItem.item.material.unit];
            }

            if (transferItem.item.type === ITEMTYPE.EQUIPMENT) {
                itemObj.description = transferItem.item.equipment.description;
                itemObj.units = [transferItem.item.equipment.unit];
                itemObj.equipmentModelId = transferItem.equipmentModelId;
                itemObj.equipmentAssets = transferItem.transferEquipmentAssets;
            }

            items.push(itemObj);
        }

        setAddedItems(items);
        setIsLoading(false);
    }, [query.data]);

    useEffect(() => {
        if (transfer.status === undefined) return;
        if (
            transfer.status === TRANSFERSTATUS.REQUESTED &&
            (auth.data.employee.employeeSiteId !== transfer.sendSiteId ||
                !auth.data.userRole.canApproveTransfer)
        ) {
            setViewOnly(true);
            return setNotificationType(TRANSFERSTATUS.REQUESTED);
        }

        if (
            transfer.status === TRANSFERSTATUS.APPROVED &&
            (auth.data.employee.employeeSiteId !== transfer.sendSiteId ||
                !auth.data.userRole.canSendTransfer)
        ) {
            setViewOnly(true);
            return setNotificationType(TRANSFERSTATUS.APPROVED);
        }

        if (
            transfer.status === TRANSFERSTATUS.SENT &&
            (auth.data.employee.employeeSiteId !== transfer.receiveSiteId ||
                !auth.data.userRole.canReceiveTransfer)
        ) {
            setViewOnly(true);
            return setNotificationType(TRANSFERSTATUS.SENT);
        }

        setViewOnly(false);
        setNotificationType(null);
    }, [transfer]);

    function TransferNotificaion() {
        switch (notificationType) {
            case TRANSFERSTATUS.REQUESTED:
                return (
                    <AlertNotification
                        title="Tranfer Needs Approval"
                        content="Transfer Has Been Requested and Needs Approval to Continue."
                    />
                );

            case TRANSFERSTATUS.APPROVED:
                return (
                    <AlertNotification
                        title="Tranfer Out Pending"
                        content="Transfer Has Been Approved but Hasn't Been Transferred Out Yet."
                    />
                );

            case TRANSFERSTATUS.SENT:
                return (
                    <AlertNotification
                        title="Tranfer In Pending"
                        content="Transfer Has Been Transferred Out but Hasn't Been Transferred In Yet."
                    />
                );

            default:
                return <></>;
        }
    }

    function TopForm() {
        return (
            <Form>
                <FormRow
                    labelL="Transfer Number"
                    valueL={transfer.transferId}
                    labelR="Status"
                    valueR={TransferStatusBadge(transfer.status)}
                />

                <FormRow
                    labelL="Sending Site"
                    valueL={transfer.sendSite.name}
                    labelR="Receiving Site"
                    valueR={transfer.receiveSite.name}
                />

                <FormRow
                    labelL="Requested By"
                    valueL={`${transfer.requestedBy.fName} ${transfer.requestedBy.mName}`}
                    labelR="Requested On"
                    valueR={new Date(transfer.requestDate).toLocaleString()}
                />

                {transfer.status >= TRANSFERSTATUS.APPROVED && (
                    <>
                        <FormRow
                            labelL="Approved By"
                            valueL={
                                transfer.approvedBy
                                    ? `${transfer.approvedBy.fName} ${transfer.approvedBy.mName}`
                                    : ""
                            }
                            labelR="Approved On"
                            valueR={
                                transfer.approveDate
                                    ? new Date(transfer.approveDate).toLocaleString()
                                    : ""
                            }
                        />
                    </>
                )}

                {transfer.status >= TRANSFERSTATUS.SENT && (
                    <>
                        <FormRow
                            labelL="Sent By"
                            valueL={
                                transfer.sentBy
                                    ? `${transfer.sentBy.fName} ${transfer.sentBy.mName}`
                                    : ""
                            }
                            labelR="Sent On"
                            valueR={
                                transfer.sendDate
                                    ? new Date(transfer.sendDate).toLocaleString()
                                    : ""
                            }
                        />
                    </>
                )}

                {transfer.status >= TRANSFERSTATUS.RECEIVED && (
                    <>
                        <FormRow
                            labelL="Received By"
                            valueL={
                                transfer.receivedBy
                                    ? `${transfer.receivedBy.fName} ${transfer.receivedBy.mName}`
                                    : ""
                            }
                            labelR="Received On"
                            valueR={
                                transfer.receiveDate
                                    ? new Date(transfer.receiveDate).toLocaleString()
                                    : ""
                            }
                        />

                        <FormRow
                            labelL="Delivered By"
                            valueL={transfer.deliveredBy}
                            labelR="Vehicle Plate No."
                            valueR={transfer.vehiclePlateNo}
                        />
                    </>
                )}
            </Form>
        );
    }

    const titles = useMemo(
        () => [
            "Declined Transfer",
            "Approve Transfer",
            "Transfer Out",
            "Transfer In",
            "Completed Transfer",
        ],
        []
    );

    if (query.isError) return <ConnectionError status={query.error?.response?.status} />;

    if (isLoading) return <LoadingSpinner />;

    return (
        <>
            <Header
                title={titles[transfer.status]}
                showPrint={transfer.status === TRANSFERSTATUS.RECEIVED || viewOnly}
            />

            <Container className="my-3">
                <>
                    <TransferNotificaion />
                    <TopForm />

                    {transfer.status === TRANSFERSTATUS.DECLINED && (
                        <DeclinedTransfer
                            addedItems={addedItems}
                            FormRow={FormRow}
                            transfer={transfer}
                        />
                    )}

                    {transfer.status === TRANSFERSTATUS.REQUESTED && !viewOnly && (
                        <ApproveTransfer
                            addedItems={addedItems}
                            setAddedItems={setAddedItems}
                            transfer={transfer}
                        />
                    )}

                    {transfer.status === TRANSFERSTATUS.APPROVED && !viewOnly && (
                        <SendTransfer
                            addedItems={addedItems}
                            setAddedItems={setAddedItems}
                            transfer={transfer}
                        />
                    )}

                    {transfer.status === TRANSFERSTATUS.SENT && !viewOnly && (
                        <ReceiveTransfer
                            addedItems={addedItems}
                            setAddedItems={setAddedItems}
                            transfer={transfer}
                        />
                    )}

                    {(transfer.status === TRANSFERSTATUS.RECEIVED || viewOnly) && (
                        <ViewTransfer addedItems={addedItems} transfer={transfer} />
                    )}
                </>
            </Container>
        </>
    );
}

export default SingleTransfer;
