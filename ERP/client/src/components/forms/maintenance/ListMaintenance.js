import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { FaFolderOpen } from "react-icons/fa";
import Table from "../../fragments/Table";
import Header from "../../layouts/Header";
import { Container } from "react-bootstrap";
import MaintenanceStatusBadge from "./fragments/MaintenanceStatusBadge";
import { useQuery } from "react-query";
import { fetchMaintenances } from "../../../api/maintenance";
import ConnectionError from "../../fragments/ConnectionError";
import LoadingSpinner from "../../fragments/LoadingSpinner";

function ListMaintenance() {
    const [maintenances, setMaintenances] = useState([]);

    var { data, isLoading, isError, error } = useQuery("maintenances", fetchMaintenances);

    useEffect(() => {
        if (data === undefined) return;

        Object.keys(data).map((key, index) => {
            //format date
            data[key].requestDate = new Date(data[key].requestDate).toLocaleString();

            //add status badge
            data[key].statusBadge = MaintenanceStatusBadge(data[key].status);

            //add links to each row
            data[key].open = (
                <>
                    <Link
                        className="btn btn-teal me-2"
                        to={`/maintenance/${data[key].maintenanceId}`}
                    >
                        <FaFolderOpen className="me-1 mb-1" />
                        Open
                    </Link>
                </>
            );

            return null;
        });

        setMaintenances(data);
    }, [data]);

    const columns = React.useMemo(
        () => [
            {
                Header: "Mnt. No.",
                accessor: "maintenanceId", // accessor is the "key" in the data
            },
            {
                Header: "Requested On",
                accessor: "requestDate",
            },
            {
                Header: "Site",
                accessor: "site.name",
            },
            {
                Header: "Status",
                accessor: "statusBadge",
                sortType: (
                    rowA,
                    rowB //get original data of row and compare status
                ) => (rowA.original.status > rowB.original.status ? 1 : -1),
            },
            {
                Header: "",
                accessor: "open", // accessor is the "key" in the data
                disableSortBy: true,
            },
        ],
        []
    );

    if (isError) return <ConnectionError status={error?.response?.status} />;

    if (isLoading) return <LoadingSpinner />;

    return (
        <>
            <Header title="Maintenances" />

            <Container className="my-3">
                <Table columns={columns} data={maintenances} />
            </Container>
        </>
    );
}

export default ListMaintenance;
