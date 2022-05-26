import React from "react";
import PropTypes from "prop-types";
import BorrowApproveEquipment from "./fragments/BorrowApproveEquipment";
import { Button, Spinner, Form } from "react-bootstrap";
import { approveBorrow, declineBorrow } from "../../../api/borrow";
import { useMutation, useQueryClient } from "react-query";
import ConnectionError from "../../fragments/ConnectionError";

function ApproveBorrow({ addedItems, setAddedItems, borrow }) {
    const queryClient = useQueryClient();

    const {
        isLoading: isApproveLoading,
        isError: isApproveError,
        error: approveError,
        mutate: submitApproveBorrow,
    } = useMutation(approveBorrow, {
        onSuccess: (res) => queryClient.invalidateQueries(["borrow", borrow.borrowId.toString()]),
    });

    const {
        isLoading: isDeclineLoading,
        isError: isDeclineError,
        error: declineError,
        mutate: submitDeclineBorrow,
    } = useMutation(declineBorrow, {
        onSuccess: (res) => queryClient.invalidateQueries(["borrow", borrow.borrowId.toString()]),
    });

    function submit(e) {
        e.preventDefault();
        if (isApproveLoading || isDeclineLoading) return;

        const type = e.nativeEvent.submitter.name;

        const data = {
            borrowId: borrow.borrowId,
            borrowItems: [],
        };

        for (let item of addedItems) {
            let tempItem = {
                itemId: item.itemId,
                equipmentModelId: item.equipmentModelId,
                qtyApproved: item.qtyApproved,
                approveRemark: item.approveRemark,
            };

            data.borrowItems.push(tempItem);
        }

        if (type === "decline") submitDeclineBorrow(data);
        else submitApproveBorrow(data);
    }

    if (isApproveError || isDeclineError)
        return (
            <ConnectionError
                status={approveError?.response?.status && declineError?.response?.status}
            />
        );

    return (
        <Form onSubmit={submit}>
            {addedItems.map((item, index) => (
                <BorrowApproveEquipment
                    key={index}
                    addedItems={addedItems}
                    setAddedItems={setAddedItems}
                    index={index}
                />
            ))}

            <div className="row">
                <div className="col-8 d-grid">
                    <Button type="submit" name="approve" className="btn-teal-dark">
                        {isApproveLoading && (
                            <Spinner className="me-2" animation="grow" size="sm" />
                        )}
                        Approve
                    </Button>
                </div>
                <div className="col-4 d-grid">
                    <Button type="submit" variant="danger" name="decline">
                        {isDeclineLoading && (
                            <Spinner className="me-2" animation="grow" size="sm" />
                        )}
                        Decline
                    </Button>
                </div>
            </div>
        </Form>
    );
}

ApproveBorrow.propTypes = {
    addedItems: PropTypes.array.isRequired,
    setAddedItems: PropTypes.func.isRequired,
    borrow: PropTypes.object.isRequired,
};

export default ApproveBorrow;
