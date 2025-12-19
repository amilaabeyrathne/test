import { Alert, Chip, Paper, Typography } from "@mui/material";
import { useEffect } from "react";
import { useGetAccountQuery } from "../services/backendApi";
import { Transaction } from "../services/types";

type TransactionItemProps = {
    transaction: Transaction
    showCurrentBalance: boolean
};

export function TransactionItem(props: TransactionItemProps) {

    const { data: accountData, refetch } = useGetAccountQuery(
        props.transaction.account_id,
        { skip: !props.showCurrentBalance }
    );
   
    const isDeposit = props.transaction.amount >= 0;

    useEffect(() => {
        if (props.showCurrentBalance) {
            refetch();
        }
    }, [props.showCurrentBalance, refetch]);

    return (
        <Paper 
            key={props.transaction.transaction_id}
            variant="outlined"
            sx={{ p: 2, borderRadius: 2 }}
            data-type="transaction"
            data-account-id={props.transaction.account_id}
            data-amount={props.transaction.amount}
            data-balance={accountData?.balance}
        > 
            <Chip 
                label={isDeposit ? 'Transaction amount (deposit)' : 'Transaction amount (withdrawal)'} 
                color={isDeposit ? 'success' : 'error'} 
                size="small" 
                sx={{ mb: 1 }}
            />
            <Typography>
                Transferred {props.transaction.amount}$ {isDeposit ? 'to' : 'from'} account {props.transaction.account_id}
            </Typography>
            {props.showCurrentBalance && (
                <Alert severity="info" sx={{ mt: 1 }}>
                    The current account balance is {accountData?.balance}
                </Alert>
            )}
        </Paper>
    );
}