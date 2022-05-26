import { useEffect, useState } from "react";
import { Form, Button, Container, Spinner } from "react-bootstrap";
import { FaPlus } from "react-icons/fa";
import Header from "../../layouts/Header";
import { ToastContainer, toast } from "react-toastify";
import { useQuery, useMutation } from "react-query";
import { fetchSites } from "../../../api/site";
import { fetchItem, importAssets } from "../../../api/item";
import { fetchEquipmentCategories, fetchEquipmentCategory } from "../../../api/category";
import LoadingSpinner from "../../fragments/LoadingSpinner";
import ConnectionError from "../../fragments/ConnectionError";
import * as XLSX from "xlsx";

function initFileObject() {
    return {
        name: "",
        file: null,
        parsed: null,
        data: [],
        sheets: [],
        assetNo: "",
        serialNo: "",
        sheet: "",
        error: "",
    };
}

function AssetNumberImport() {
    const [categoryId, setCategoryId] = useState("");
    const [siteId, setSiteId] = useState(0);
    const [itemId, setItemId] = useState(0);
    const [modelId, setModelId] = useState(0);
    const [file, setFile] = useState(initFileObject);

    const categoriesQuery = useQuery("categories", fetchEquipmentCategories);
    var itemsQuery = useQuery(
        ["equipmentcategory", categoryId],
        () => categoryId && fetchEquipmentCategory(categoryId)
    );
    var itemQuery = useQuery(["item", itemId], () => itemId && fetchItem(itemId));

    const sitesQuery = useQuery("sites", fetchSites);

    const toastOption = {
        position: "bottom-right",
        autoClose: 5000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        theme: "dark",
        // progress: undefined
    };

    const {
        isError: isSubmitError,
        error: submitError,
        isLoading: isSubmitLoading,
        mutate: submitEquipmentRequest,
    } = useMutation(importAssets, {
        onSuccess: (res) => {
            toast.success(`${res} Assets Successfully Added`, toastOption);
        },
    });

    useEffect(() => {
        if (!file.file || file.name === "") return;
        const fileReader = new FileReader();
        fileReader.readAsArrayBuffer(file.file);

        fileReader.onload = (e) => {
            try {
                const parsed = XLSX.read(e.target.result, { type: "buffer" });
                const sheets = parsed.SheetNames;

                setFile((prev) => ({ ...prev, parsed, sheets, sheet: sheets[0] }));
            } catch {
                setFile((prev) => ({
                    ...prev,
                    error: "Could Not Parse File, Please Upload Another File",
                }));
            }
        };
    }, [file.name, file.file]);

    useEffect(() => {
        if (!file.sheet) return;

        const sheet = file.parsed.Sheets[file.sheet];
        const data = XLSX.utils.sheet_to_json(sheet);
        var assetNo = "";

        try {
            assetNo = Object.keys(data[0])[0];
        } catch {
            setFile((prev) => ({
                ...prev,
                name: "",
                error: "Could Not Parse File (Selected Sheet), Please Upload Another File",
            }));
        }

        setFile((prev) => ({ ...prev, data, assetNo }));
    }, [file.sheet]);

    function submit(e) {
        e.preventDefault();
        if (isSubmitLoading) return;

        const assets = file.data.map((row) => ({
            assetNo: String(row[file.assetNo]),
            serialNo: String(row[file.serialNo]),
        }));

        var data = {
            equipmentModelId: modelId,
            siteId: siteId,
            assets,
        };

        submitEquipmentRequest(data);
        setFile(initFileObject());
    }

    if (
        categoriesQuery.isError ||
        itemsQuery.isError ||
        itemQuery.isError ||
        sitesQuery.isError ||
        isSubmitError
    )
        return (
            <ConnectionError
                status={
                    categoriesQuery?.error?.response?.status ??
                    itemsQuery?.error?.response?.status ??
                    itemQuery?.error?.response?.status ??
                    sitesQuery?.error?.response?.status ??
                    submitError?.response?.status
                }
            />
        );

    return (
        <>
            <Header title="Import Asset Numbers" />

            <Container className="my-3 align-self-center">
                {(categoriesQuery.isLoading ||
                    itemsQuery.isLoading ||
                    itemQuery.isLoading ||
                    sitesQuery.isLoading) && <LoadingSpinner />}

                <Form onSubmit={submit}>
                    <div className="col-10 shadow py-5 px-5 mx-auto rounded">
                        <div className="row">
                            <div className="col">
                                <Form.Group className="mb-3">
                                    <Form.Label>Import Assets To Site</Form.Label>

                                    <Form.Select
                                        value={siteId}
                                        onChange={(e) => setSiteId(e.target.value)}
                                        required
                                    >
                                        <option value="">Select Site</option>
                                        {sitesQuery.data?.map((site) => (
                                            <option key={site.siteId} value={site.siteId}>
                                                {site.name}
                                            </option>
                                        ))}
                                    </Form.Select>
                                </Form.Group>
                            </div>
                        </div>
                        <div className="row">
                            <div className="col">
                                <Form.Group className="mb-3">
                                    <Form.Label>Category</Form.Label>
                                    <Form.Select
                                        value={categoryId}
                                        onChange={(e) => setCategoryId(e.target.value)}
                                        required
                                    >
                                        <option value="">Select Equipment Category</option>
                                        {categoriesQuery.data?.map((category) => (
                                            <option
                                                key={category.equipmentCategoryId}
                                                value={category.equipmentCategoryId}
                                            >
                                                {category.name}
                                            </option>
                                        ))}
                                    </Form.Select>
                                </Form.Group>
                            </div>
                        </div>
                        <div className="row">
                            <div className="col">
                                <Form.Group className="mb-3">
                                    <Form.Label>Item</Form.Label>
                                    <Form.Select
                                        value={itemId}
                                        onChange={(e) => setItemId(e.target.value)}
                                        required
                                    >
                                        <option value="">Select Equipment</option>
                                        {itemsQuery.data?.equipments?.map((equipment) => (
                                            <option key={equipment.itemId} value={equipment.itemId}>
                                                {equipment.item.name}
                                            </option>
                                        ))}
                                    </Form.Select>
                                </Form.Group>
                            </div>

                            <div className="col">
                                <Form.Group className="mb-3">
                                    <Form.Label>Model</Form.Label>
                                    <Form.Select
                                        value={modelId}
                                        onChange={(e) => setModelId(e.target.value)}
                                        required
                                    >
                                        <option value="">Select Model</option>
                                        {itemQuery.data?.equipment?.equipmentModels?.map(
                                            (equipmentModel) => (
                                                <option
                                                    key={equipmentModel.equipmentModelId}
                                                    value={equipmentModel.equipmentModelId}
                                                >
                                                    {equipmentModel.name}
                                                </option>
                                            )
                                        )}
                                    </Form.Select>
                                </Form.Group>
                            </div>
                        </div>

                        <div className="row">
                            <div className="col">
                                <Form.Group className="mb-3">
                                    <Form.Label>Asset Number File</Form.Label>
                                    <Form.Control
                                        type="file"
                                        onChange={(e) => {
                                            setFile((prev) => ({
                                                ...initFileObject(),
                                                name: e.target.value,
                                                file: e.target.files[0],
                                            }));
                                        }}
                                        required
                                        accept=".xls, .xlsx"
                                    />
                                </Form.Group>
                            </div>
                        </div>
                        {file.error ? (
                            <h1 className="display-6 fs-5"> {file.error}</h1>
                        ) : (
                            <>
                                {file.sheets.length !== 0 && (
                                    <div className="col">
                                        <Form.Group className="mb-3">
                                            <Form.Label>Sheet</Form.Label>
                                            <Form.Select
                                                value={file.sheet}
                                                onChange={(e) =>
                                                    setFile((prev) => ({
                                                        ...prev,
                                                        sheet: e.target.value,
                                                    }))
                                                }
                                                required
                                            >
                                                {file.sheets?.map((sheet, index) => (
                                                    <option key={`${sheet}${index}`} value={sheet}>
                                                        {sheet}
                                                    </option>
                                                ))}
                                            </Form.Select>
                                        </Form.Group>
                                    </div>
                                )}

                                {file.data.length !== 0 && (
                                    <div className="col">
                                        <Form.Group className="mb-3">
                                            <Form.Label>Asset Number Column</Form.Label>
                                            <Form.Select
                                                value={file.assetNo}
                                                onChange={(e) =>
                                                    setFile((prev) => ({
                                                        ...prev,
                                                        assetNo: e.target.value,
                                                    }))
                                                }
                                                required
                                            >
                                                {Object.keys(file.data[0])
                                                    ?.filter((column) => column !== file.serialNo)
                                                    ?.map((column, index) => (
                                                        <option
                                                            key={`${column}${index}`}
                                                            value={column}
                                                        >
                                                            {column}
                                                        </option>
                                                    ))}
                                            </Form.Select>
                                        </Form.Group>
                                    </div>
                                )}

                                {file.data.length !== 0 && (
                                    <div className="col">
                                        <Form.Group className="mb-3">
                                            <Form.Label>Serial Number Column</Form.Label>
                                            <Form.Select
                                                value={file.serialNo}
                                                onChange={(e) =>
                                                    setFile((prev) => ({
                                                        ...prev,
                                                        serialNo: e.target.value,
                                                    }))
                                                }
                                                required
                                            >
                                                <option value="">No Serial Number Column</option>
                                                {Object.keys(file.data[0])
                                                    ?.filter((column) => column !== file.assetNo)
                                                    ?.map((column, index) => (
                                                        <option
                                                            key={`${column}${index}`}
                                                            value={column}
                                                        >
                                                            {column}
                                                        </option>
                                                    ))}
                                            </Form.Select>
                                        </Form.Group>
                                    </div>
                                )}
                                {file.assetNo && modelId ? (
                                    <div className="row">
                                        <div className="d-grid">
                                            <Button type="submit" className="btn-teal">
                                                {isSubmitLoading ? (
                                                    <Spinner
                                                        className="me-2"
                                                        animation="grow"
                                                        size="sm"
                                                    />
                                                ) : (
                                                    <FaPlus className="me-2 mb-1" />
                                                )}
                                                Import Asset Numbers
                                            </Button>
                                        </div>
                                    </div>
                                ) : null}
                            </>
                        )}
                    </div>
                </Form>
                <ToastContainer />
            </Container>
        </>
    );
}

export default AssetNumberImport;
