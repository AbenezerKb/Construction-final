import { useEffect, useRef, useState } from "react"
import { Form, Button, Container } from "react-bootstrap"
import { FaPlus } from "react-icons/fa"
import Item from "../../../models/Item"
import PurchaseRequestEquipment from "./fragments/PurchaseRequestEquipment"
import Header from "../../layouts/Header"
import { useQuery, useMutation } from "react-query"
import { fetchSites } from "../../../api/site"
import { fetchEquipments } from "../../../api/item"
import { requestEquipmentPurchase } from "../../../api/purchase"
import LoadingSpinner from "../../fragments/LoadingSpinner"
import ConnectionError from "../../fragments/ConnectionError"
import { useNavigate } from "react-router-dom"

function EquipmentPurchase() {
  const navigate = useNavigate()
  const [receivingSite, setReceivingSite] = useState(0)
  const [addedItems, setAddedItems] = useState([new Item()])

  const [allSites, setAllSites] = useState([])
  const [allItems, setAllItems] = useState([])
  const [isLoading, setIsLoading] = useState(true)
  const [isError, setIsError] = useState(false)

  const sitesQuery = useQuery("sites", fetchSites, {
    onSuccess: (data) => {
      setAllSites(data)
      setReceivingSite(data[0].siteId)
    },
  })
  const itemQuery = useQuery("equipments", fetchEquipments, {
    onSuccess: (data) => setAllItems(data),
  })

  useEffect(() => {
    setIsLoading(sitesQuery.isLoading || itemQuery.isLoading)
  }, [sitesQuery.isLoading, itemQuery.isLoading])

  useEffect(() => {
    setIsError(sitesQuery.isError || itemQuery.isError)
  }, [sitesQuery.isError, itemQuery.isError])

  const {
    isError: isSubmitError,
    error: submitError,
    isLoading: isSubmitLoading,
    mutate: submitPurchaseRequest,
  } = useMutation(requestEquipmentPurchase, {
    onSuccess: (res) => navigate(`/purchase/${res.data}`),
  })

  function submit(e) {
    e.preventDefault()
    if (isSubmitLoading) return

    var data = {
      receivingSiteId: Number(receivingSite),
      purchaseItems: [],
    }

    for (let item of addedItems) {
      let tempItem = {
        itemId: Number(item.itemId),
        qtyRequested: Number(item.qtyRequested),
        equipmentModelId: Number(item.equipmentModelId),
        requestRemark: item.requestRemark,
      }

      data.purchaseItems.push(tempItem)
    }

    submitPurchaseRequest(data)
  }

  //scroll when Item is added
  var pageend = useRef(null)
  const [scroll, setScroll] = useState(false)

  useEffect(() => {
    if (scroll) {
      pageend.scrollIntoView()
      setScroll(false)
    }
  }, [scroll])

  function addItem() {
    setAddedItems([...addedItems, new Item()])
    setScroll(true)
  }

  if (isError || isSubmitError)
    return (
      <ConnectionError
        status={
          itemQuery?.error?.response?.status ??
          sitesQuery?.error?.response?.status ??
          submitError?.response?.status
        }
      />
    )

  if (isLoading) return <LoadingSpinner />

  return (
    <>
      <Header title="New Equipment Purchase Request" />

      <Container className="my-3">
        <Form onSubmit={submit}>
          <div className="row mx-2">
            <div className="col">
              <Form.Group className="mb-3">
                <div className="row">
                  <div className="col-1 mt-1" style={{ width: "15%" }}>
                    <Form.Label>Receiving Site</Form.Label>
                  </div>
                  <div className="col">
                    <Form.Select
                      value={receivingSite}
                      onChange={(e) => setReceivingSite(e.target.value)}
                    >
                      {allSites.map((site) => (
                        <option key={site.siteId} value={site.siteId}>
                          {site.name}
                        </option>
                      ))}
                    </Form.Select>
                  </div>
                </div>
              </Form.Group>
            </div>
          </div>

          {addedItems.map((item, index) => (
            <PurchaseRequestEquipment
              key={item.key}
              index={index}
              allItems={allItems}
              addedItems={addedItems}
              setAddedItems={setAddedItems}
            />
          ))}

          <div className="row">
            <div className="col-9 d-grid">
              <Button type="submit" className="btn-teal-dark">
                Request Purchase
              </Button>
            </div>
            <div className="col-3 d-grid">
              <Button className="btn-teal" onClick={addItem}>
                <FaPlus className="me-2 mb-1" /> Add Item
              </Button>
            </div>
          </div>
        </Form>
        <div
          style={{ float: "left", clear: "both" }}
          ref={(el) => {
            pageend = el
          }}
        ></div>
      </Container>
    </>
  )
}

export default EquipmentPurchase
