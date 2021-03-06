import weight.src.weight.weight_main;
import laplace.laplace.src.laplace.laplace_main;
import maxlike.maxlike.src.maxlike.maxlike_main;
import cmts.src.cmts.CMTSMAIN;
import cmts_score.cmts_score.src.cmts_score.cmtsscore_main;
import java.io.*;

/**
 * <p>
 * Title:Building associative classifier with multiple minimum supports
 * </p>
 * <p>
 * Description: Use the consept of multiple minimum supports to build up
 * classifier for association rule
 * </p>
 * <p>
 * Copyright: Copyright (c) 2012
 * </p>
 * <p>
 * Company: National Chung Cheng University, Taiwan, R.O.C.
 * </p>
 * 
 * @author Jian-Shian Wang
 * @version 1.0
 */

public class aLL {

	public static void main(String[] args) {
		String filename = "balance-scale";
		int foldint = 10;

		CMTSMAIN cmtsmain = new CMTSMAIN(filename, foldint);
		System.out.println("\n各分類器預測結果 (minsup=" + cmtsmain.MIN + " sigma=" + cmtsmain.SIGMA + ")\n");

		System.out.println("----------最大概似----------");
		maxlike_main max_main = new maxlike_main(filename, foldint);
		System.out.println("\n----------Laplace----------");
		laplace_main la_main = new laplace_main(filename, foldint);
		System.out.println("\n----------Score----------");
		cmtsscore_main cm_main = new cmtsscore_main(filename, foldint);
		System.out.println("\n----------Max x----------");
		weight_main wei_main = new weight_main(filename, foldint);

		System.out.println("\n全部執行完成!!");
		//delFolder("output");
	}

	public static void delFolder(String folderPath) {
		try {
			delAllFile(folderPath); // 删除完里面所有内容
			String filePath = folderPath;
			filePath = filePath.toString();
			java.io.File myFilePath = new java.io.File(filePath);
			// myFilePath.delete(); //删除空文件夹
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	public static boolean delAllFile(String path) {
		boolean flag = false;
		File file = new File(path);
		if (!file.exists()) {
			return flag;
		}
		if (!file.isDirectory()) {
			return flag;
		}
		String[] tempList = file.list();
		File temp = null;
		for (int i = 0; i < tempList.length; i++) {
			if (path.endsWith(File.separator)) {
				temp = new File(path + tempList[i]);
			} else {
				temp = new File(path + File.separator + tempList[i]);
			}
			if (temp.isFile()) {
				temp.delete();
			}
			if (temp.isDirectory()) {
				delAllFile(path + "/" + tempList[i]);// 先删除文件夹里面的文件
				// delFolder(path + "/" + tempList[i]);//再删除空文件夹
				flag = true;
			}
		}
		return flag;
	}
}