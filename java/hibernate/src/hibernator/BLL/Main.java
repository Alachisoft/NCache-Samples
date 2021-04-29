package hibernator.BLL;

public class Main {
    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        try {
            HibernateOperations.run();
        } catch (Exception e) {
            System.err.println("StackTrace: ");
            e.printStackTrace();
            System.out.println("Error:" + e.toString());
            System.exit(0);
        }
    }
}
