import React from "react";
import PropTypes from "prop-types";
import IssueHandMaterial from "./fragments/IssueHandMaterial";
import { Button, Form, Spinner } from "react-bootstrap";
import { handIssue } from "../../../api/issue";
import { useMutation, useQueryClient } from "react-query";
import ConnectionError from "../../fragments/ConnectionError";

function HandIssue({ addedItems, setAddedItems, issue }) {
    const queryClient = useQueryClient();

    const {
        isLoading: isHandLoading,
        isError: isHandError,
        error: handError,
        mutate: submitHandIssue,
    } = useMutation(handIssue, {
        onSuccess: (res) => queryClient.invalidateQueries(["issue", issue.issueId.toString()]),
    });

    function submit(e) {
        e.preventDefault();
        if (isHandLoading) return;

        const data = {
            issueId: issue.issueId,
            handedById: 1,
            issueItems: [],
        };

        for (let item of addedItems) {
            let tempItem = {
                itemId: Number(item.itemId),
                handRemark: item.handRemark,
            };
            data.issueItems.push(tempItem);
        }

        submitHandIssue(data);
    }

    if (isHandError) return <ConnectionError status={handError?.response?.status} />;

    return (
        <>
            <Form onSubmit={submit}>
                {addedItems.map((item, index) => (
                    <IssueHandMaterial
                        key={index}
                        addedItems={addedItems}
                        setAddedItems={setAddedItems}
                        index={index}
                    />
                ))}

                <div className="d-grid">
                    <Button className="btn-teal-dark" type="submit">
                        {isHandLoading && <Spinner className="me-2" animation="grow" size="sm" />}
                        Hand Over Issued Items
                    </Button>
                </div>
            </Form>
        </>
    );
}

HandIssue.propTypes = {
    addedItems: PropTypes.array.isRequired,
    setAddedItems: PropTypes.func.isRequired,
    issue: PropTypes.object.isRequired,
};

export default HandIssue;
