import { useEffect, useState, useMemo } from "react"
import { useParams } from "react-router-dom"
import { Form, Container } from "react-bootstrap"
import Item from "../../../models/Item"
import Header from "../../layouts/Header"
import BulkPurchaseStatusBadge from "./fragments/BulkPurchaseStatusBadge"
import { ITEMTYPE, BULKPURCHASESTATUS } from "../../../Constants"
import RequestBulkPurchase from "./RequestBulkPurchase"
import ApproveBulkPurchase from "./ApproveBulkPurchase"
import ConfirmBulkPurchase from "./ConfirmBulkPurchase"
import ViewBulkPurchase from "./ViewBulkPurchase"
import DeclinedBulkPurchase from "./DeclinedBulkPurchase"
import { useQuery } from "react-query"
import FormRow from "../../fragments/FormRow"
import { fetchBulkPurchase } from "../../../api/bulkPurchase"
import { useAuth } from "../../../contexts/AuthContext"
import LoadingSpinner from "../../fragments/LoadingSpinner"
import ConnectionError from "../../fragments/ConnectionError"
import AlertNotification from "../../fragments/AlertNotification"

function SingleBulkPurchase() {
  const { id } = useParams()
  const [bulkPurchase, setBulkPurchase] = useState({})
  const [addedItems, setAddedItems] = useState([])
  const [isLoading, setIsLoading] = useState(true)
  const [viewOnly, setViewOnly] = useState()
  const [notificationType, setNotificationType] = useState(null)

  const auth = useAuth()

  var query = useQuery(["bulkPurchase", id], () => fetchBulkPurchase(id))

  useEffect(() => {
    if (query.data === undefined) return
    setBulkPurchase(query.data)
    let items = []
    for (let bulkPurchaseItem of query.data.bulkPurchaseItems) {
      let itemObj = new Item()

      itemObj.itemId = bulkPurchaseItem.itemId
      itemObj.name = bulkPurchaseItem.item.name
      itemObj.cost = bulkPurchaseItem.cost
      itemObj.type = bulkPurchaseItem.item.type

      itemObj.qtyRequested = bulkPurchaseItem.qtyRequested
      itemObj.requestRemark = bulkPurchaseItem.requestRemark

      if (
        query.data.status >= BULKPURCHASESTATUS.APPROVED ||
        query.data.status === BULKPURCHASESTATUS.DECLINED
      ) {
        itemObj.qtyApproved = bulkPurchaseItem.qtyApproved
        itemObj.approveRemark = bulkPurchaseItem.approveRemark
      }

      if (query.data.status >= BULKPURCHASESTATUS.PURCHASED) {
        itemObj.qtyPurchased = bulkPurchaseItem.qtyPurchased
        itemObj.purchaseRemark = bulkPurchaseItem.purchaseRemark
      }

      if (bulkPurchaseItem.item.type === ITEMTYPE.MATERIAL) {
        itemObj.spec = bulkPurchaseItem.item.material.spec
        itemObj.units = [bulkPurchaseItem.item.material.unit]
      }

      if (bulkPurchaseItem.item.type === ITEMTYPE.EQUIPMENT) {
        itemObj.description = bulkPurchaseItem.item.equipment.description
        itemObj.units = [bulkPurchaseItem.item.equipment.unit]
        itemObj.equipmentModelId = bulkPurchaseItem.equipmentModelId
      }

      items.push(itemObj)
    }

    setAddedItems(items)
    setIsLoading(false)
  }, [query.data])

  useEffect(() => {
    if (bulkPurchase.status === undefined) return
    if (
      bulkPurchase.status === BULKPURCHASESTATUS.QUEUED &&
      !auth.data.userRole.canRequestBulkPurchase
    ) {
      setViewOnly(true)
      return setNotificationType(BULKPURCHASESTATUS.QUEUED)
    }

    if (
      bulkPurchase.status === BULKPURCHASESTATUS.REQUESTED &&
      !auth.data.userRole.canApproveBulkPurchase
    ) {
      setViewOnly(true)
      return setNotificationType(BULKPURCHASESTATUS.REQUESTED)
    }

    if (
      bulkPurchase.status === BULKPURCHASESTATUS.APPROVED &&
      !auth.data.userRole.canConfirmBulkPurchase
    ) {
      setViewOnly(true)
      return setNotificationType(BULKPURCHASESTATUS.APPROVED)
    }

    setViewOnly(false)
    setNotificationType(null)
  }, [bulkPurchase])

  function BulkPurchaseNotificaion() {
    switch (notificationType) {
      case BULKPURCHASESTATUS.QUEUED:
        return (
          <AlertNotification
            title="BulkPurchase Needs Request"
            content="BulkPurchase Has Been Queued and Needs Request to Continue."
          />
        )

      case BULKPURCHASESTATUS.REQUESTED:
        return (
          <AlertNotification
            title="BulkPurchase Needs Approval"
            content="BulkPurchase Has Been Requested and Needs Approval to Continue."
          />
        )

      case BULKPURCHASESTATUS.APPROVED:
        return (
          <AlertNotification
            title="BulkPurchase Confirmation Pending"
            content="BulkPurchase Has Been Approved but Hasn't Been Purchased Yet."
          />
        )

      default:
        return <></>
    }
  }

  function TopForm() {
    return (
      <Form>
        <FormRow
          labelL="BulkPurchase Number"
          valueL={bulkPurchase.bulkPurchaseId}
          labelR="Status"
          valueR={BulkPurchaseStatusBadge(bulkPurchase.status)}
        />

        <FormRow
          labelL="Requested By"
          valueL={`${bulkPurchase.requestedBy.fName} ${bulkPurchase.requestedBy.mName}`}
          labelR="Requested On"
          valueR={new Date(bulkPurchase.requestDate).toLocaleString()}
        />

        {bulkPurchase.status >= BULKPURCHASESTATUS.APPROVED && (
          <>
            <FormRow
              labelL="Approved By"
              valueL={
                bulkPurchase.approvedBy
                  ? `${bulkPurchase.approvedBy.fName} ${bulkPurchase.approvedBy.mName}`
                  : ""
              }
              labelR="Approved On"
              valueR={
                bulkPurchase.approveDate
                  ? new Date(bulkPurchase.approveDate).toLocaleString()
                  : ""
              }
            />
          </>
        )}
      </Form>
    )
  }

  const titles = useMemo(
    () => [
      "Declined BulkPurchase",
      "Request BulkPurchase",
      "Approve BulkPurchase",
      "Confirm BulkPurchase",
      "BulkItems PURCHASED",
    ],
    []
  )

  if (query.isError)
    return <ConnectionError status={query.error?.response?.status} />

  if (isLoading) return <LoadingSpinner />

  return (
    <>
      <Header
        title={titles[bulkPurchase.status]}
        showPrint={
          bulkPurchase.status === BULKPURCHASESTATUS.PURCHASED || viewOnly
        }
      />

      <Container className="my-3">
        <>
          <BulkPurchaseNotificaion />

          <TopForm />

          {bulkPurchase.status === BULKPURCHASESTATUS.DECLINED && (
            <DeclinedBulkPurchase
              addedItems={addedItems}
              FormRow={FormRow}
              bulkPurchase={bulkPurchase}
            />
          )}

          {bulkPurchase.status === BULKPURCHASESTATUS.QUEUED && !viewOnly && (
            <RequestBulkPurchase
              addedItems={addedItems}
              setAddedItems={setAddedItems}
              bulkPurchase={bulkPurchase}
            />
          )}

          {bulkPurchase.status === BULKPURCHASESTATUS.REQUESTED &&
            !viewOnly && (
              <ApproveBulkPurchase
                addedItems={addedItems}
                setAddedItems={setAddedItems}
                bulkPurchase={bulkPurchase}
              />
            )}

          {bulkPurchase.status === BULKPURCHASESTATUS.APPROVED && !viewOnly && (
            <ConfirmBulkPurchase
              addedItems={addedItems}
              setAddedItems={setAddedItems}
              bulkPurchase={bulkPurchase}
            />
          )}

          {(bulkPurchase.status === BULKPURCHASESTATUS.PURCHASED ||
            viewOnly) && (
            <ViewBulkPurchase
              addedItems={addedItems}
              bulkPurchase={bulkPurchase}
            />
          )}
        </>
      </Container>
    </>
  )
}

export default SingleBulkPurchase
