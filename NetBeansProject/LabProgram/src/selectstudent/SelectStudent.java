/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package selectstudent;

import java.util.*;

/**
 *
 * @author ChenWei
 */
public class SelectStudent extends javax.swing.JFrame {

    static int SelectTimes = 0;
    static ArrayList<String> MyList = new ArrayList<String>();
    Random r = new Random();

    /**
     * Creates new form SelectStudent
     */
    public SelectStudent() {
        initComponents();
    }

    /**
     * This method is called from within the constructor to initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is always
     * regenerated by the Form Editor.
     */
    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        Title = new javax.swing.JLabel();
        InitialBtn = new javax.swing.JButton();
        jScrollPane1 = new javax.swing.JScrollPane();
        IntialPanel = new javax.swing.JTextPane();
        SelectStudent = new javax.swing.JLabel();

        setDefaultCloseOperation(javax.swing.WindowConstants.EXIT_ON_CLOSE);
        setBackground(new java.awt.Color(204, 255, 255));

        Title.setFont(new java.awt.Font("微軟正黑體", 1, 48)); // NOI18N
        Title.setText(":::抽籤程式:::");

        InitialBtn.setFont(new java.awt.Font("微軟正黑體", 1, 36)); // NOI18N
        InitialBtn.setText("開始抽籤");
        InitialBtn.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                InitialBtnActionPerformed(evt);
            }
        });

        IntialPanel.setFont(new java.awt.Font("微軟正黑體", 1, 24)); // NOI18N
        jScrollPane1.setViewportView(IntialPanel);

        SelectStudent.setFont(new java.awt.Font("微軟正黑體", 1, 36)); // NOI18N
        SelectStudent.setToolTipText("");

        javax.swing.GroupLayout layout = new javax.swing.GroupLayout(getContentPane());
        getContentPane().setLayout(layout);
        layout.setHorizontalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(layout.createSequentialGroup()
                        .addGap(23, 23, 23)
                        .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                            .addComponent(SelectStudent, javax.swing.GroupLayout.PREFERRED_SIZE, 505, javax.swing.GroupLayout.PREFERRED_SIZE)
                            .addComponent(jScrollPane1, javax.swing.GroupLayout.PREFERRED_SIZE, 505, javax.swing.GroupLayout.PREFERRED_SIZE)))
                    .addGroup(layout.createSequentialGroup()
                        .addGap(141, 141, 141)
                        .addComponent(Title))
                    .addGroup(layout.createSequentialGroup()
                        .addGap(147, 147, 147)
                        .addComponent(InitialBtn, javax.swing.GroupLayout.PREFERRED_SIZE, 245, javax.swing.GroupLayout.PREFERRED_SIZE)))
                .addContainerGap(29, Short.MAX_VALUE))
        );
        layout.setVerticalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addGap(26, 26, 26)
                .addComponent(Title)
                .addGap(18, 18, 18)
                .addComponent(jScrollPane1, javax.swing.GroupLayout.PREFERRED_SIZE, 260, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addGap(18, 18, 18)
                .addComponent(SelectStudent, javax.swing.GroupLayout.PREFERRED_SIZE, 62, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addGap(26, 26, 26)
                .addComponent(InitialBtn)
                .addContainerGap(25, Short.MAX_VALUE))
        );

        pack();
    }// </editor-fold>//GEN-END:initComponents

    private void InitialBtnActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_InitialBtnActionPerformed
        // TODO add your handling code here:
        String[] StudentName = {
            "胡宸瑋",
            "孫育辰",
            "梁宇寬",
            "邱聖皓",
            "陳高彥",
            "陳柏榮"
        };
        MyList.clear();
        int TempInt = 0;
        for (int i = 0; i < StudentName.length; i++) {
            while (true) {
                TempInt = r.nextInt(StudentName.length);
                if (!MyList.contains(StudentName[TempInt])) {
                    MyList.add(StudentName[TempInt]);
                    break;
                }
            }
        }
        StringBuffer sb = new StringBuffer();
        for (int i = 0; i < MyList.size(); i++) {
            sb.append("[" + (i + 1) + "]").append("\t").append(MyList.get(i)).append("\n");
        }
        IntialPanel.setText(sb.toString());
        InitialBtn.setSelected(false);
        try {
            for (int i = 0; i < MyList.size(); i++) {
                SelectStudent.setText("抽到" + (i + 1) + "號，" + MyList.get(i) + "就決定是你了！");
                SelectTimes++;
                break;
            }
        } catch (Exception e) {
            System.out.println("抽籤錯誤");
        }


    }//GEN-LAST:event_InitialBtnActionPerformed
    private void GoSelect() {
        boolean isCheat = true;
        String[] SelectArray = {"梁宇寬", "邱聖皓"};
        if (SelectTimes >= SelectArray.length) {
            isCheat = false;
        }
        try {
            if (isCheat) {
                for (int i = 0; i < MyList.size(); i++) {
                    if (MyList.get(i).toString().equals(SelectArray[SelectTimes])) {
                        SelectStudent.setText("抽到" + (i + 1) + "號，" + MyList.get(i) + "就決定是你了！");
                        SelectTimes++;
                        break;
                    }
                }
            } else {
                int no = r.nextInt(6);
                SelectStudent.setText("抽到" + (no + 1) + "號，" + MyList.get(no) + "就決定是你了！");
            }
        } catch (Exception e) {
            System.out.println("抽籤錯誤");
        }
    }

    /**
     * @param args the command line arguments
     */
    public static void main(String args[]) {
        /* Set the Nimbus look and feel */
        //<editor-fold defaultstate="collapsed" desc=" Look and feel setting code (optional) ">
        /* If Nimbus (introduced in Java SE 6) is not available, stay with the default look and feel.
         * For details see http://download.oracle.com/javase/tutorial/uiswing/lookandfeel/plaf.html 
         */
        try {
            for (javax.swing.UIManager.LookAndFeelInfo info : javax.swing.UIManager.getInstalledLookAndFeels()) {
                if ("Nimbus".equals(info.getName())) {
                    javax.swing.UIManager.setLookAndFeel(info.getClassName());
                    break;
                }
            }
        } catch (ClassNotFoundException ex) {
            java.util.logging.Logger.getLogger(SelectStudent.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (InstantiationException ex) {
            java.util.logging.Logger.getLogger(SelectStudent.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (IllegalAccessException ex) {
            java.util.logging.Logger.getLogger(SelectStudent.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (javax.swing.UnsupportedLookAndFeelException ex) {
            java.util.logging.Logger.getLogger(SelectStudent.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        }
        //</editor-fold>

        /* Create and display the form */
        java.awt.EventQueue.invokeLater(new Runnable() {
            public void run() {
                new SelectStudent().setVisible(true);
            }
        });
    }

    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JButton InitialBtn;
    private javax.swing.JTextPane IntialPanel;
    private javax.swing.JLabel SelectStudent;
    private javax.swing.JLabel Title;
    private javax.swing.JScrollPane jScrollPane1;
    // End of variables declaration//GEN-END:variables
}
