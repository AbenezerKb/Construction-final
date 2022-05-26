import { useEffect, useState, useMemo } from "react";
import { useParams } from "react-router-dom";
import { Form, Container } from "react-bootstrap";
import Item from "../../../models/Item";
import Header from "../../layouts/Header";
import BorrowStatusBadge from "./fragments/BorrowStatusBadge";
import { BORROWSTATUS, ITEMTYPE } from "../../../Constants";
import ApproveBorrow from "./ApproveBorrow";
import HandBorrow from "./HandBorrow";
import ViewBorrow from "./ViewBorrow";
import DeclinedBorrow from "./DeclinedBorrow";
import { useQuery } from "react-query";
import LoadingSpinner from "../../fragments/LoadingSpinner";
import ConnectionError from "../../fragments/ConnectionError";
import { fetchBorrow } from "../../../api/borrow";
import { useAuth } from "../../../contexts/AuthContext";
import AlertNotification from "../../fragments/AlertNotification";

function SingleBorrow() {
    const { id } = useParams();
    const [borrow, setBorrow] = useState({});
    const [addedItems, setAddedItems] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [viewOnly, setViewOnly] = useState();
    const [notificationType, setNotificationType] = useState(null);
    const auth = useAuth();

    var query = useQuery(["borrow", id], () => fetchBorrow(id));

    useEffect(() => {
        if (query.data === undefined) return;
        setBorrow(query.data);
        let items = [];
        for (let borrowItem of query.data.borrowItems) {
            let itemObj = new Item();

            itemObj.itemId = borrowItem.itemId;
            itemObj.name = borrowItem.item.name;
            itemObj.cost = borrowItem.cost;

            itemObj.qtyRequested = borrowItem.qtyRequested;
            itemObj.requestRemark = borrowItem.requestRemark;

            if (
                query.data.status >= BORROWSTATUS.APPROVED ||
                query.data.status === BORROWSTATUS.DECLINED
            ) {
                itemObj.qtyApproved = borrowItem.qtyApproved;
                itemObj.approveRemark = borrowItem.approveRemark;
            }

            if (query.data.status >= BORROWSTATUS.HANDED) {
                itemObj.handRemark = borrowItem.handRemark;
            }

            if (borrowItem.item.type === ITEMTYPE.EQUIPMENT) {
                itemObj.equipmentModelId = borrowItem.equipmentModelId;
                itemObj.equipmentAssets = borrowItem.borrowEquipmentAssets;
                itemObj.description = borrowItem.item.equipment.description;
                itemObj.units = [borrowItem.item.equipment.unit];
            }

            items.push(itemObj);
        }

        setAddedItems(items);
        setIsLoading(false);
    }, [query.data]);

    useEffect(() => {
        if (borrow.status === undefined) return;
        if (
            borrow.status === BORROWSTATUS.REQUESTED &&
            (auth.data.employee.employeeSiteId !== borrow.siteId ||
                !auth.data.userRole.canApproveBorrow)
        ) {
            setViewOnly(true);
            return setNotificationType(BORROWSTATUS.REQUESTED);
        }

        if (
            borrow.status === BORROWSTATUS.APPROVED &&
            (auth.data.employee.employeeSiteId !== borrow.siteId ||
                !auth.data.userRole.canHandBorrow)
        ) {
            setViewOnly(true);
            return setNotificationType(BORROWSTATUS.APPROVED);
        }

        setViewOnly(false);
        setNotificationType(null);
    }, [borrow]);

    function BorrowNotificaion() {
        switch (notificationType) {
            case BORROWSTATUS.REQUESTED:
                return (
                    <AlertNotification
                        title="Borrow Request Needs Approval"
                        content="Borrow Has Been Requested and Needs Approval to Continue."
                    />
                );

            case BORROWSTATUS.APPROVED:
                return (
                    <AlertNotification
                        title="Handing Borrowed Items Pending"
                        content="Borrow Has Been Approved but Hasn't Been Handed Over Yet."
                    />
                );

            default:
                return <></>;
        }
    }

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
                    labelL="Borrow Number"
                    valueL={borrow.borrowId}
                    labelR="Status"
                    valueR={BorrowStatusBadge(borrow.status)}
                />

                <div className="row mx-2">
                    <div className="col">
                        <Form.Group className="mb-3">
                            <div className="row">
                                <div className="col-1 mt-1" style={{ width: "12.499999995%" }}>
                                    <Form.Label>Issue Site</Form.Label>
                                </div>
                                <div className="col">
                                    <Form.Control type="text" readOnly value={borrow.site.name} />
                                </div>
                            </div>
                        </Form.Group>
                    </div>
                </div>

                <FormRow
                    labelL="Issued By"
                    valueL={`${borrow.requestedBy.fName} ${borrow.requestedBy.mName}`}
                    labelR="Issued On"
                    valueR={new Date(borrow.requestDate).toLocaleString()}
                />

                {borrow.status >= BORROWSTATUS.APPROVED && (
                    <>
                        <FormRow
                            labelL="Approved By"
                            valueL={
                                borrow.approvedBy
                                    ? `${borrow.approvedBy.fName} ${borrow.approvedBy.mName}`
                                    : ""
                            }
                            labelR="Approved On"
                            valueR={
                                borrow.approveDate
                                    ? new Date(borrow.approveDate).toLocaleString()
                                    : ""
                            }
                        />
                    </>
                )}

                {borrow.status >= BORROWSTATUS.HANDED && (
                    <>
                        <FormRow
                            labelL="Handed Over By"
                            valueL={
                                borrow.handedBy
                                    ? `${borrow.handedBy.fName} ${borrow.handedBy.mName}`
                                    : ""
                            }
                            labelR="Handed Over On"
                            valueR={
                                borrow.handDate ? new Date(borrow.handDate).toLocaleString() : ""
                            }
                        />
                    </>
                )}
            </Form>
        );
    }

    const titles = useMemo(
        () => [
            "Declined Borrow Request",
            "Approve Borrow Request",
            "Hand Over Borrow Request",
            "Completed Borrow Request",
        ],
        []
    );

    if (query.isError) return <ConnectionError status={query.error?.response?.status} />;

    if (isLoading) return <LoadingSpinner />;

    return (
        <>
            <Header
                title={titles[borrow.status]}
                showPrint={borrow.status === BORROWSTATUS.HANDED || viewOnly}
            />

            <Container className="my-3">
                <>
                    <BorrowNotificaion />
                    <TopForm />

                    {borrow.status === BORROWSTATUS.DECLINED && (
                        <DeclinedBorrow addedItems={addedItems} FormRow={FormRow} borrow={borrow} />
                    )}

                    {borrow.status === BORROWSTATUS.REQUESTED && !viewOnly && (
                        <ApproveBorrow
                            addedItems={addedItems}
                            setAddedItems={setAddedItems}
                            borrow={borrow}
                        />
                    )}

                    {borrow.status === BORROWSTATUS.APPROVED && !viewOnly && (
                        <HandBorrow
                            addedItems={addedItems}
                            setAddedItems={setAddedItems}
                            borrow={borrow}
                        />
                    )}

                    {(borrow.status === BORROWSTATUS.HANDED || viewOnly) && (
                        <ViewBorrow addedItems={addedItems} />
                    )}
                </>
            </Container>
        </>
    );
}

export default SingleBorrow;
