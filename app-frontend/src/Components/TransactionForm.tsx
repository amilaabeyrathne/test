import { Alert, Box, Button, Stack, TextField, Typography } from "@mui/material"
import { useState } from "react"
import { useCreateTransactionMutation, useLazyListTransactionsQuery } from "../services/backendApi";

const guidRegex =
  /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/;

type FormErrors = {
  accountId?: string;
  amount?: string;
  submit?: string;
};

export function TransactionForm() {
  const [accountId, setAccountId] = useState("");
  const [amount, setAmount] = useState("");
  const [errors, setErrors] = useState<FormErrors>({});
  const [createTransaction] = useCreateTransactionMutation();
  const [listTransactions] = useLazyListTransactionsQuery(); 
  
  const validateAccountId = (value: string) => {
    const trimmed = value.trim();
    if (!trimmed) {
      return "Account ID is required.";
    }
    if (!guidRegex.test(trimmed)) {
      return "Account ID must be a valid GUID.";
    }
    return undefined;
  };

  const validateAmount = (value: string) => {
    const trimmed = value.trim();
    if (!trimmed) {
      return "Amount is required.";
    }
    const numericValue = Number(trimmed);
    if (!Number.isFinite(numericValue)) {
      return "Amount must be a number.";
    }
    if (!Number.isInteger(numericValue)) {
      return "Amount must be an integer.";
    }
    if (numericValue === 0) {
      return "Amount cannot be zero.";
    }
    return undefined;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const accountError = validateAccountId(accountId);
    const amountError = validateAmount(amount);
    if (accountError || amountError) {
      setErrors({ accountId: accountError, amount: amountError });
      return;
    }

    try {
      await createTransaction({
        account_id: accountId.trim(),
        amount: Number(amount.trim()),
      }).unwrap();

      // Clear form fields after successful submission
      setAccountId("");
      setAmount("");
      setErrors({});
      // Refresh transaction list after submission
      await listTransactions();
    } catch (err) {
      console.error("Failed to create transaction:", err);
      setErrors((prev) => ({
        ...prev,
        submit:
          "We couldn’t create the transaction right now. Please try again.",
      }));
    }
  };

  return (
    <Box component="form" noValidate onSubmit={handleSubmit}>
      <Typography variant="h5" sx={{ mb: 2 }}>
        Submit new transaction
      </Typography>
      <Stack spacing={2}>
        <TextField
          label="Account ID"
          value={accountId}
          onChange={(e) => {
            const value = e.target.value;
            setAccountId(value);

            setErrors((prev) => ({
              ...prev,
              accountId: prev.accountId ? validateAccountId(value) : undefined,
              submit: undefined,
            }));
          }}
          onBlur={(e) => {
            setErrors((prev) => ({
              ...prev,
              accountId: validateAccountId(e.target.value),
            }));
          }}
          error={Boolean(errors.accountId)}
          helperText={errors.accountId}
          inputProps={{ "data-type": "account-id" }}
          fullWidth
        />
        <TextField
          label="Amount"
          type="number"
          value={amount}
          onChange={(e) => {
            const value = e.target.value;
            setAmount(value);

            setErrors((prev) => ({
              ...prev,
              amount: prev.amount ? validateAmount(value) : undefined,
              submit: undefined,
            }));
          }}
          onBlur={(e) => {
            const value = e.target.value;
            setErrors((prev) => ({
              ...prev,
              amount: validateAmount(value),
            }));
          }}
          error={Boolean(errors.amount)}
          helperText={errors.amount}
          inputProps={{ "data-type": "amount", step: 1 }}
          fullWidth
        />
        <Button
          type="submit"
          variant="contained"
          fullWidth
          data-type="transaction-submit"
          disabled={
            Boolean(errors.accountId) ||
            Boolean(errors.amount) ||
            Boolean(errors.submit)
          }
        >
          Submit
        </Button>
        {errors.submit ? (
          <Alert severity="error" data-type="transaction-error">
            {errors.submit}
          </Alert>
        ) : null}
      </Stack>
    </Box>
  );
}