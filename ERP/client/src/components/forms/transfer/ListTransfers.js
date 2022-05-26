import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { FaFolderOpen } from "react-icons/fa";
import Table from "../../fragments/Table";
import Header from "../../layouts/Header";
import { Container } from "react-bootstrap";
import TransferStatusBadge from "./fragments/TransferStatusBadge";
import PropTypes from "prop-types";
import { useQuery } from "react-query";
import { fetchTransfers } from "../../../api/transfer";
import ConnectionError from "../../fragments/ConnectionError";
import LoadingSpinner from "../../fragments/LoadingSpinner";
import { useAuth } from "../../../contexts/AuthContext";

function ListTransfers({ title, type }) {
    const [transfers, setTransfers] = useState([]);
    const auth = useAuth();

    const requestData = {};
    if (type == "in") requestData["receiveSiteId"] = auth.data.employee.employeeSiteId;
    else requestData["sendSiteId"] = auth.data.employee.employeeSiteId;

    var { data, isLoading, isError, error } = useQuery(["transfers", type], () =>
        fetchTransfers(requestData)
    );

    useEffect(() => {
        if (data === undefined) return;

        Object.keys(data).map((key, index) => {
            //format date
            data[key].requestDate = new Date(data[key].requestDate).toLocaleString();

            //add status badge
            data[key].statusBadge = TransferStatusBadge(data[key].status);

            //add links to each row
            data[key].open = (
                <>
                    <Link className="btn btn-teal me-2" to={`/transfer/${data[key].transferId}`}>
                        <FaFolderOpen className="me-1 mb-1" />
                        Open
                    </Link>
                </>
            );

            return null;
        });

        setTransfers(data);
    }, [data]);

    const columns = React.useMemo(
        () => [
            {
                Header: "Transfer No.",
                accessor: "transferId", // accessor is the "key" in the data
            },
            {
                Header: "Requested On",
                accessor: "requestDate",
            },
            {
                Header: "Sending Site",
                accessor: "sendSite.name",
            },
            {
                Header: "Receiving Site",
                accessor: "receiveSite.name",
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

    if (isLoading) return <LoadingSpinner />;

    if (isError) return <ConnectionError status={error?.response?.status} />;

    return (
        <>
            <Header title={title} />

            <Container className="my-3">
                <Table columns={columns} data={transfers} />
            </Container>
        </>
    );
}

ListTransfers.propTypes = {
    title: PropTypes.string.isRequired,
    type: PropTypes.string.isRequired,
};

export default ListTransfers;
