import React from "react";
import PropTypes from "prop-types";
import MaintenanceFixEquipment from "./fragments/MaintenanceFixEquipment";
import { Button, Form, Spinner } from "react-bootstrap";
import { fixMaintenance } from "../../../api/maintenance";
import { useMutation, useQueryClient } from "react-query";
import ConnectionError from "../../fragments/ConnectionError";

function FixMaintenance({ addedItems, setAddedItems, maintenance }) {
    const queryClient = useQueryClient();

    const {
        isLoading: isHandLoading,
        isError: isHandError,
        error: handError,
        mutate: submitFixMaintenance,
    } = useMutation(fixMaintenance, {
        onSuccess: (res) =>
            queryClient.invalidateQueries(["maintenance", maintenance.maintenanceId.toString()]),
    });

    function submit(e) {
        e.preventDefault();
        if (isHandLoading) return;

        const data = {
            maintenanceId: maintenance.maintenanceId,
            fixRemark: addedItems[0].fixRemark,
        };

        submitFixMaintenance(data);
    }

    if (isHandError) return <ConnectionError submit={handError?.response?.status} />;

    return (
        <>
            <Form onSubmit={submit}>
                {addedItems.map((item, index) => (
                    <MaintenanceFixEquipment
                        key={index}
                        addedItems={addedItems}
                        setAddedItems={setAddedItems}
                        index={index}
                    />
                ))}

                <div className="d-grid">
                    <Button className="btn-teal-dark" type="submit">
                        {isHandLoading && <Spinner className="me-2" animation="grow" size="sm" />}
                        Mark Item As Fixed
                    </Button>
                </div>
            </Form>
        </>
    );
}

FixMaintenance.propTypes = {
    addedItems: PropTypes.array.isRequired,
    setAddedItems: PropTypes.func.isRequired,
    maintenance: PropTypes.object.isRequired,
};

export default FixMaintenance;
