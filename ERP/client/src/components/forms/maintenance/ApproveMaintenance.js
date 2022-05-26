import React from "react";
import PropTypes from "prop-types";
import MaintenanceApproveEquipment from "./fragments/MaintenanceApproveEquipment";
import { Button, Spinner } from "react-bootstrap";
import { approveMaintenance, declineMaintenance } from "../../../api/maintenance";
import { useMutation, useQueryClient } from "react-query";
import ConnectionError from "../../fragments/ConnectionError";

function ApproveMaintenance({ addedItems, setAddedItems, maintenance }) {
    const queryClient = useQueryClient();

    const {
        isLoading: isApproveLoading,
        isError: isApproveError,
        error: approveError,
        mutate: submitApproveMaintenance,
    } = useMutation(approveMaintenance, {
        onSuccess: (res) =>
            queryClient.invalidateQueries(["maintenance", maintenance.maintenanceId.toString()]),
    });

    const {
        isLoading: isDeclineLoading,
        isError: isDeclineError,
        error: declineError,
        mutate: submitDeclineMaintenance,
    } = useMutation(declineMaintenance, {
        onSuccess: (res) =>
            queryClient.invalidateQueries(["maintenance", maintenance.maintenanceId.toString()]),
    });

    function submit(type, e) {
        e.preventDefault();
        if (isApproveLoading || isDeclineLoading) return;

        const data = {
            maintenanceId: maintenance.maintenanceId,
            approveRemark: addedItems[0].approveRemark,
        };

        if (type === "decline") submitDeclineMaintenance(data);
        else submitApproveMaintenance(data);
    }

    if (isApproveError || isDeclineError)
        return (
            <ConnectionError
                status={approveError?.response?.status ?? declineError?.response?.status}
            />
        );

    return (
        <>
            {addedItems.map((item, index) => (
                <MaintenanceApproveEquipment
                    key={index}
                    addedItems={addedItems}
                    setAddedItems={setAddedItems}
                    index={index}
                />
            ))}

            <div className="row">
                <div className="col-8 d-grid">
                    <Button className="btn-teal-dark" onClick={submit.bind(this, "approve")}>
                        {isApproveLoading && (
                            <Spinner className="me-2" animation="grow" size="sm" />
                        )}
                        Approve
                    </Button>
                </div>
                <div className="col-4 d-grid">
                    <Button variant="danger" onClick={submit.bind(this, "decline")}>
                        {isDeclineLoading && (
                            <Spinner className="me-2" animation="grow" size="sm" />
                        )}
                        Decline
                    </Button>
                </div>
            </div>
        </>
    );
}

ApproveMaintenance.propTypes = {
    addedItems: PropTypes.array.isRequired,
    setAddedItems: PropTypes.func.isRequired,
    maintenance: PropTypes.object.isRequired,
};

export default ApproveMaintenance;
