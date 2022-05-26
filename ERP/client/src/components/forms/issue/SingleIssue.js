import { useEffect, useState, useMemo } from "react";
import { useParams } from "react-router-dom";
import { Form, Container } from "react-bootstrap";
import Item from "../../../models/Item";
import Header from "../../layouts/Header";
import IssueStatusBadge from "./fragments/IssueStatusBadge";
import { ISSUESTATUS, ITEMTYPE } from "../../../Constants";
import ApproveIssue from "./ApproveIssue";
import HandIssue from "./HandIssue";
import ViewIssue from "./ViewIssue";
import DeclinedIssue from "./DeclinedIssue";
import { useQuery } from "react-query";
import FormRow from "../../fragments/FormRow";
import { fetchIssue } from "../../../api/issue";
import LoadingSpinner from "../../fragments/LoadingSpinner";
import ConnectionError from "../../fragments/ConnectionError";
import { useAuth } from "../../../contexts/AuthContext";
import AlertNotification from "../../fragments/AlertNotification";

function SingleIssue() {
    const { id } = useParams();
    const [issue, setIssue] = useState({});
    const [addedItems, setAddedItems] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [viewOnly, setViewOnly] = useState(true);
    const [notificationType, setNotificationType] = useState(null);
    const auth = useAuth();
    var query = useQuery(["issue", id], () => fetchIssue(id));

    useEffect(() => {
        if (query.data === undefined) return;
        setIssue(query.data);
        let items = [];
        for (let issueItem of query.data.issueItems) {
            let itemObj = new Item();

            itemObj.itemId = issueItem.itemId;
            itemObj.name = issueItem.item.name;
            itemObj.cost = issueItem.cost;

            itemObj.qtyRequested = issueItem.qtyRequested;
            itemObj.requestRemark = issueItem.requestRemark;

            if (
                query.data.status >= ISSUESTATUS.APPROVED ||
                query.data.status === ISSUESTATUS.DECLINED
            ) {
                itemObj.qtyApproved = issueItem.qtyApproved;
                itemObj.approveRemark = issueItem.approveRemark;
            }

            if (query.data.status >= ISSUESTATUS.HANDED) {
                itemObj.handRemark = issueItem.handRemark;
            }

            if (issueItem.item.type === ITEMTYPE.MATERIAL) {
                itemObj.spec = issueItem.item.material.spec;
                itemObj.units = [issueItem.item.material.unit];
            }

            items.push(itemObj);
        }

        setAddedItems(items);
        setIsLoading(false);
    }, [query.data]);

    useEffect(() => {
        if (issue.status === undefined) return;
        if (
            issue.status === ISSUESTATUS.REQUESTED &&
            (auth.data.employee.employeeSiteId !== issue.siteId ||
                !auth.data.userRole.canApproveIssue)
        ) {
            setViewOnly(true);
            return setNotificationType(ISSUESTATUS.REQUESTED);
        }

        if (
            issue.status === ISSUESTATUS.APPROVED &&
            (auth.data.employee.employeeSiteId !== issue.siteId || !auth.data.userRole.canHandIssue)
        ) {
            setViewOnly(true);
            return setNotificationType(ISSUESTATUS.APPROVED);
        }

        setViewOnly(false);
        setNotificationType(null);
    }, [issue]);

    function IssueNotificaion() {
        switch (notificationType) {
            case ISSUESTATUS.REQUESTED:
                return (
                    <AlertNotification
                        title="Issue Request Needs Approval"
                        content="Issue Has Been Requested and Needs Approval to Continue."
                    />
                );

            case ISSUESTATUS.APPROVED:
                return (
                    <AlertNotification
                        title="Handing Issued Items Pending"
                        content="Issue Has Been Approved but Hasn't Been Handed Over Yet."
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
                    labelL="Issue Number"
                    valueL={issue.issueId}
                    labelR="Status"
                    valueR={IssueStatusBadge(issue.status)}
                />

                <div className="row mx-2">
                    <div className="col">
                        <Form.Group className="mb-3">
                            <div className="row">
                                <div className="col-1 mt-1" style={{ width: "12.499999995%" }}>
                                    <Form.Label>Issue Site</Form.Label>
                                </div>
                                <div className="col">
                                    <Form.Control type="text" readOnly value={issue.site.name} />
                                </div>
                            </div>
                        </Form.Group>
                    </div>
                </div>

                <FormRow
                    labelL="Issued By"
                    valueL={`${issue.requestedBy.fName} ${issue.requestedBy.mName}`}
                    labelR="Issued On"
                    valueR={new Date(issue.requestDate).toLocaleString()}
                />

                {issue.status >= ISSUESTATUS.APPROVED && (
                    <>
                        <FormRow
                            labelL="Approved By"
                            valueL={
                                issue.approvedBy
                                    ? `${issue.approvedBy.fName} ${issue.approvedBy.mName}`
                                    : ""
                            }
                            labelR="Approved On"
                            valueR={
                                issue.approveDate
                                    ? new Date(issue.approveDate).toLocaleString()
                                    : ""
                            }
                        />
                    </>
                )}

                {issue.status >= ISSUESTATUS.HANDED && (
                    <>
                        <FormRow
                            labelL="Handed Over By"
                            valueL={
                                issue.handedBy
                                    ? `${issue.handedBy.fName} ${issue.handedBy.mName}`
                                    : ""
                            }
                            labelR="Handed Over On"
                            valueR={issue.handDate ? new Date(issue.handDate).toLocaleString() : ""}
                        />
                    </>
                )}
            </Form>
        );
    }

    const titles = useMemo(
        () => ["Declined Issue", "Approve Issue", "Hand Issue", "Completed Issue"],
        []
    );

    if (query.isError) return <ConnectionError status={query?.error?.response?.status} />;

    if (isLoading) return <LoadingSpinner />;

    return (
        <>
            <Header
                title={titles[issue.status]}
                showPrint={issue.status === ISSUESTATUS.HANDED || viewOnly}
            />

            <Container className="my-3">
                <>
                    <IssueNotificaion />
                    <TopForm />

                    {issue.status === ISSUESTATUS.DECLINED && (
                        <DeclinedIssue addedItems={addedItems} FormRow={FormRow} issue={issue} />
                    )}

                    {issue.status === ISSUESTATUS.REQUESTED && !viewOnly && (
                        <ApproveIssue
                            addedItems={addedItems}
                            setAddedItems={setAddedItems}
                            issue={issue}
                        />
                    )}

                    {issue.status === ISSUESTATUS.APPROVED && !viewOnly && (
                        <HandIssue
                            addedItems={addedItems}
                            setAddedItems={setAddedItems}
                            issue={issue}
                        />
                    )}

                    {(issue.status === ISSUESTATUS.HANDED || viewOnly) && (
                        <ViewIssue
                            addedItems={addedItems}
                            setAddedItems={setAddedItems}
                            issue={issue}
                        />
                    )}
                </>
            </Container>
        </>
    );
}

export default SingleIssue;
