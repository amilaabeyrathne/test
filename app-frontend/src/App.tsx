
import { Box, Container, Divider, Paper, Stack, Typography } from "@mui/material"
import { TransactionForm } from "./Components/TransactionForm";
import { TransactionList } from "./Components/TransactionList";

function App() {

  return (
   <Container maxWidth="lg" sx={{ py: 4 }}>
      <Typography variant="h4" sx={{ mb: 3 }}>Transaction Management</Typography>
      <Box sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr', md: '5fr 0fr 7fr' }, gap: 2, alignItems: 'start' }}>
        <Box>
          <Paper variant="outlined" sx={{ p: 3, height: '100%', borderRadius: 2 }}>
            <TransactionForm />
          </Paper>
        </Box>
        <Box sx={{ display: { xs: 'none', md: 'block' } }}>
          <Divider orientation="vertical" flexItem />
        </Box>
        <Box>
          <Stack spacing={2}>
            <TransactionList />
          </Stack>
        </Box>
      </Box>
   </Container>
  )
}

export default App;
