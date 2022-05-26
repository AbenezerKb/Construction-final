import { useEffect, useState, useMemo } from "react"
import { useParams } from "react-router-dom"
import { Form, Container } from "react-bootstrap"
import Item from "../../../models/Item"
import Header from "../../layouts/Header"
import PurchaseStatusBadge from "./fragments/PurchaseStatusBadge"
import { ITEMTYPE, PURCHASESTATUS } from "../../../Constants"
import ApprovePurchase from "./ApprovePurchase"
import ConfirmPurchase from "./ConfirmPurchase"
import RequestQueuedPurchase from "./RequestQueuedPurchase"
import CheckPurchase from "./CheckPurchase"
import ViewPurchase from "./ViewPurchase"
import DeclinedPurchase from "./DeclinedPurchase"
import { useQuery } from "react-query"
import FormRow from "../../fragments/FormRow"
import { fetchPurchase } from "../../../api/purchase"
import { useAuth } from "../../../contexts/AuthContext"
import LoadingSpinner from "../../fragments/LoadingSpinner"
import ConnectionError from "../../fragments/ConnectionError"
import AlertNotification from "../../fragments/AlertNotification"

function SinglePurchase() {
  const { id } = useParams()
  const [purchase, setPurchase] = useState({})
  const [addedItems, setAddedItems] = useState([])
  const [isLoading, setIsLoading] = useState(true)
  const [viewOnly, setViewOnly] = useState()
  const [notificationType, setNotificationType] = useState(null)

  const auth = useAuth()

  var query = useQuery(["purchase", id], () => fetchPurchase(id))

  useEffect(() => {
    if (query.data === undefined) return
    setPurchase(query.data)
    let items = []
    for (let purchaseItem of query.data.purchaseItems) {
      let itemObj = new Item()

      itemObj.itemId = purchaseItem.itemId
      itemObj.name = purchaseItem.item.name
      itemObj.cost = purchaseItem.cost
      itemObj.type = purchaseItem.item.type

      itemObj.qtyRequested = purchaseItem.qtyRequested
      itemObj.requestRemark = purchaseItem.requestRemark

      if (
        query.data.status >= PURCHASESTATUS.APPROVED ||
        query.data.status === PURCHASESTATUS.DECLINED
      ) {
        itemObj.qtyApproved = purchaseItem.qtyApproved
        itemObj.approveRemark = purchaseItem.approveRemark
      }

      if (query.data.status >= PURCHASESTATUS.PURCHASED) {
        itemObj.qtyPurchased = purchaseItem.qtyPurchased
        itemObj.purchaseRemark = purchaseItem.purchaseRemark
      }

      if (purchaseItem.item.type === ITEMTYPE.MATERIAL) {
        itemObj.spec = purchaseItem.item.material.spec
        itemObj.units = [purchaseItem.item.material.unit]
      }

      if (purchaseItem.item.type === ITEMTYPE.EQUIPMENT) {
        itemObj.description = purchaseItem.item.equipment.description
        itemObj.units = [purchaseItem.item.equipment.unit]
        itemObj.equipmentModelId = purchaseItem.equipmentModelId
      }

      items.push(itemObj)
    }

    setAddedItems(items)
    setIsLoading(false)
  }, [query.data])

  useEffect(() => {
    if (purchase.status === undefined) return
    if (purchase.status === PURCHASESTATUS.BULKQUEUED) {
      setViewOnly(true)
      return setNotificationType(PURCHASESTATUS.BULKQUEUED)
    }
    if (
      purchase.status === PURCHASESTATUS.QUEUED &&
      (auth.data.employee.employeeSiteId !== purchase.receivingSiteId ||
        !auth.data.userRole.canRequestPurchase)
    ) {
      setViewOnly(true)
      return setNotificationType(PURCHASESTATUS.QUEUED)
    }

    if (
      purchase.status === PURCHASESTATUS.REQUESTED &&
      (auth.data.employee.employeeSiteId !== purchase.receivingSiteId ||
        !auth.data.userRole.canCheckPurchase)
    ) {
      setViewOnly(true)
      return setNotificationType(PURCHASESTATUS.REQUESTED)
    }

    if (
      purchase.status === PURCHASESTATUS.CHECKED &&
      (auth.data.employee.employeeSiteId !== purchase.receivingSiteId ||
        !auth.data.userRole.canApprovePurchase)
    ) {
      setViewOnly(true)
      return setNotificationType(PURCHASESTATUS.CHECKED)
    }

    if (
      purchase.status === PURCHASESTATUS.APPROVED &&
      (auth.data.employee.employeeSiteId !== purchase.receivingSiteId ||
        !auth.data.userRole.canConfirmPurchase)
    ) {
      setViewOnly(true)
      return setNotificationType(PURCHASESTATUS.APPROVED)
    }

    setViewOnly(false)
    setNotificationType(null)
  }, [purchase])

  function PurchaseNotificaion() {
    switch (notificationType) {
      case PURCHASESTATUS.QUEUED:
        return (
          <AlertNotification
            title="Purchase Needs Request"
            content="Purchase Has Been Queued and Needs Request to Continue."
          />
        )

      case PURCHASESTATUS.REQUESTED:
        return (
          <AlertNotification
            title="Purchase Needs Check"
            content="Purchase Has Been Requested and Needs Check to Continue."
          />
        )

      case PURCHASESTATUS.BULKQUEUED:
        return (
          <AlertNotification
            title="Purchase Pending"
            content="Purchase Has Been Queued For Bulk Purchase, Wait For Bulk Purchase."
          />
        )

      case PURCHASESTATUS.CHECKED:
        return (
          <AlertNotification
            title="Purchase Needs Approval"
            content="Purchase Has Been Checked and Needs Approval to Continue."
          />
        )

      case PURCHASESTATUS.APPROVED:
        return (
          <AlertNotification
            title="Purchase Confirmation Pending"
            content="Purchase Has Been Approved but Hasn't Been Purchased Yet."
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
          labelL="Purchase Number"
          valueL={purchase.purchaseId}
          labelR="Status"
          valueR={PurchaseStatusBadge(purchase.status)}
        />

        <FormRow labelL="Receiving Site" valueL={purchase.receivingSite.name} />

        <FormRow
          labelL="Requested By"
          valueL={`${purchase.requestedBy.fName} ${purchase.requestedBy.mName}`}
          labelR="Requested On"
          valueR={new Date(purchase.requestDate).toLocaleString()}
        />

        {purchase.status >= PURCHASESTATUS.CHECKED && (
          <>
            <FormRow
              labelL="Checked By"
              valueL={
                purchase.checkedBy
                  ? `${purchase.checkedBy.fName} ${purchase.checkedBy.mName}`
                  : ""
              }
              labelR="Checked On"
              valueR={
                purchase.checkDate
                  ? new Date(purchase.checkDate).toLocaleString()
                  : ""
              }
            />
          </>
        )}

        {purchase.status >= PURCHASESTATUS.APPROVED && (
          <>
            <FormRow
              labelL="Approved By"
              valueL={
                purchase.approvedBy
                  ? `${purchase.approvedBy.fName} ${purchase.approvedBy.mName}`
                  : ""
              }
              labelR="Approved On"
              valueR={
                purchase.approveDate
                  ? new Date(purchase.approveDate).toLocaleString()
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
      "Declined Purchase",
      "Request Purchase",
      "Check Purchase",
      "Queued For Bulk Purchase",
      "Approve Purchase",
      "Confirm Purchase",
      "Item Purchased",
    ],
    []
  )

  if (query.isError)
    return <ConnectionError status={query.error?.response?.status} />

  if (isLoading) return <LoadingSpinner />

  return (
    <>
      <Header
        title={titles[purchase.status]}
        showPrint={purchase.status === PURCHASESTATUS.PURCHASED || viewOnly}
      />

      <Container className="my-3">
        <>
          <PurchaseNotificaion />

          <TopForm />

          {purchase.status === PURCHASESTATUS.DECLINED && (
            <DeclinedPurchase
              addedItems={addedItems}
              FormRow={FormRow}
              purchase={purchase}
            />
          )}

          {purchase.status === PURCHASESTATUS.QUEUED && !viewOnly && (
            <RequestQueuedPurchase
              addedItems={addedItems}
              setAddedItems={setAddedItems}
              purchase={purchase}
            />
          )}

          {purchase.status === PURCHASESTATUS.REQUESTED && !viewOnly && (
            <CheckPurchase
              addedItems={addedItems}
              setAddedItems={setAddedItems}
              purchase={purchase}
            />
          )}

          {purchase.status === PURCHASESTATUS.CHECKED && !viewOnly && (
            <ApprovePurchase
              addedItems={addedItems}
              setAddedItems={setAddedItems}
              purchase={purchase}
            />
          )}

          {purchase.status === PURCHASESTATUS.APPROVED && !viewOnly && (
            <ConfirmPurchase
              addedItems={addedItems}
              setAddedItems={setAddedItems}
              purchase={purchase}
            />
          )}

          {(purchase.status === PURCHASESTATUS.PURCHASED || viewOnly) && (
            <ViewPurchase addedItems={addedItems} purchase={purchase} />
          )}
        </>
      </Container>
    </>
  )
}

export default SinglePurchase
