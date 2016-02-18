package cmts.src.cmts;

import java.io.Serializable;
import java.util.Comparator;

public class compareF implements Comparator<freqdatacase>,Serializable{

	@Override
	public int compare(freqdatacase o1, freqdatacase o2) {
		// TODO Auto-generated method stub
		if(o1.getMinsup() > o2.getMinsup()){
			return 1;
		}else{
			return -1;
		}
	}
}
