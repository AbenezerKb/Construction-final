import React from "react";
import PropTypes from "prop-types";
import BorrowHandEquipment from "./fragments/BorrowHandEquipment";
import { Button, Form, Spinner } from "react-bootstrap";
import { handBorrow } from "../../../api/borrow";
import { useMutation, useQueryClient } from "react-query";
import ConnectionError from "../../fragments/ConnectionError";

function HandBorrow({ addedItems, setAddedItems, borrow }) {
    const queryClient = useQueryClient();

    const {
        isLoading: isHandLoading,
        isError: isHandError,
        error: handError,
        mutate: submitHandBorrow,
    } = useMutation(handBorrow, {
        onSuccess: (res) => queryClient.invalidateQueries(["borrow", borrow.borrowId.toString()]),
    });

    function submit(e) {
        e.preventDefault();
        if (isHandLoading) return;

        const data = {
            borrowId: borrow.borrowId,
            borrowItems: [],
        };

        for (let item of addedItems) {
            let tempItem = {
                itemId: item.itemId,
                equipmentModelId: item.equipmentModelId,
                handRemark: item.handRemark,
                equipmentAssetIds: item.equipmentAssets.map((ea) => ea.value),
            };

            data.borrowItems.push(tempItem);
        }

        submitHandBorrow(data);
    }

    if (isHandError) return <ConnectionError status={handError?.response?.status} />;

    return (
        <>
            <Form onSubmit={submit}>
                {addedItems.map((item, index) => (
                    <BorrowHandEquipment
                        key={index}
                        addedItems={addedItems}
                        setAddedItems={setAddedItems}
                        index={index}
                    />
                ))}

                <div className="d-grid">
                    <Button className="btn-teal-dark" type="submit">
                        {isHandLoading && <Spinner className="me-2" animation="grow" size="sm" />}
                        Hand Over Borrow Request Items
                    </Button>
                </div>
            </Form>
        </>
    );
}

HandBorrow.propTypes = {
    addedItems: PropTypes.array.isRequired,
    setAddedItems: PropTypes.func.isRequired,
    borrow: PropTypes.object.isRequired,
};

export default HandBorrow;
